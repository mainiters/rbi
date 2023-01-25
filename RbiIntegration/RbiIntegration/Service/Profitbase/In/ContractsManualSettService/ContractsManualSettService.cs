using RbiIntegration.Service.BaseClasses;
using RbiIntegration.Service.Profitbase.In.ContractsManualSettService.Model.Request;
using RbiIntegration.Service.Profitbase.In.ContractsManualSettService.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.Entities;
using Terrasoft.Web.Common;
using static RbiIntegration.Service.BaseClasses.CrmConstants;

namespace RbiIntegration.Service.Profitbase.In.ContractsManualSettService
{
    /// <summary>
    /// Сервис получения текущего статуса взаиморасчетов
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class ContractsManualSettService : BaseRbiService<ContractsManualSettServiceRequestModel, ContractsManualSettServiceResponseModel>
    {
        public ContractsManualSettService(UserConnection UserConnection) : base(UserConnection)
        {
            this.UserConnection = UserConnection;
        }

        protected override Guid GetIntegrationServiceId(ContractsManualSettServiceRequestModel requestModel)
        {
            return CrmConstants.TrcIntegrationServices.ContractsManualSett;
        }

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        protected override ContractsManualSettServiceResponseModel ProcessBusinessLogic(ContractsManualSettServiceRequestModel requestModel, ContractsManualSettServiceResponseModel response)
        {
            Entity contract = null;
            Entity request = null;

            try
            {
                var esq = new EntitySchemaQuery(this.UserConnection.EntitySchemaManager, "Contract");

                esq.AddAllSchemaColumns();

                esq.AddColumn("TrcType.Name");
                esq.AddColumn("State.Name");
                esq.AddColumn("Parent.Number");

                esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Id", requestModel.contractId));

                contract = esq.GetEntityCollection(this.UserConnection).FirstOrDefault();

                if (contract == null)
                {
                    response.Code = 500;
                    response.Result = false;
                    response.ReasonPhrase = $"Запись с id {requestModel.contractId} не найдена";
                    return response;
                }

                esq = new EntitySchemaQuery(this.UserConnection.EntitySchemaManager, "TrcRequest");

                esq.AddAllSchemaColumns();

                esq.AddColumn("TrcObjectProfitbase.TrcObjectId");
                esq.AddColumn("TrcOpportunity.TrcObject.TrcObjectId");

                esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcContractRequest", requestModel.contractId));
                esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcRequestType", Guid.Parse("512F0D01-99C1-4C1B-8AD2-9DCD4C56ABC6")));
                esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcService", Guid.Parse("82983928-3428-4201-B44F-E181F711873D")));
                esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.NotEqual, "TrcRequestStatus", Guid.Parse("DB6398B8-7805-4A47-8857-50FBD798207A")));

                request = esq.GetEntityCollection(this.UserConnection).FirstOrDefault();

                if (request != null)
                {
                    var TrcObjectId = string.Empty;

                    if (request.GetColumnValue("TrcObjectProfitbaseId") != null)
                    {
                        TrcObjectId = request.GetTypedColumnValue<string>("TrcObjectProfitbase_TrcObjectId");
                    }
                    else
                    {
                        TrcObjectId = request.GetTypedColumnValue<string>("TrcOpportunity_TrcObject_TrcObjectId");
                    }

                    if (!string.IsNullOrEmpty(TrcObjectId))
                    {
                        int val;

                        if (int.TryParse(TrcObjectId, out val))
                        {
                            response.objectId = val;
                        }
                    }

                    if (request.GetTypedColumnValue<float>("TrcProjectArea") != 0
                        && request.GetTypedColumnValue<float>("TrcAreaPIB") != 0
                        && request.GetTypedColumnValue<float>("TrcDeviationInSquareMeters") != 0
                        && request.GetTypedColumnValue<float>("TrcAmountDeviation") != 0)
                    {
                        var measurementsData = new List<KeyValueData>();

                        measurementsData.Add(new KeyValueData()
                        {
                            name = "Плановая площадь",
                            value = request.GetTypedColumnValue<string>("TrcProjectArea").Replace(",00", "")
                        });

                        measurementsData.Add(new KeyValueData()
                        {
                            name = "Фактическая площадь",
                            value = request.GetTypedColumnValue<string>("TrcAreaPIB").Replace(",00", "")
                        });

                        var TrcDeviationInSquareMeters = request.GetTypedColumnValue<float>("TrcDeviationInSquareMeters");

                        measurementsData.Add(new KeyValueData()
                        {
                            name = "Разница площадей",
                            value = Math.Abs(TrcDeviationInSquareMeters).ToString().Replace(",00", "")
                        });

                        if (TrcDeviationInSquareMeters < -1)
                        {
                            measurementsData.Add(new KeyValueData()
                            {
                                name = "Сумма доплаты",
                                value = Math.Abs(request.GetTypedColumnValue<float>("TrcAmountDeviation")).ToString().Replace(",00", "")
                            });
                        }
                        else if(TrcDeviationInSquareMeters > 1)
                        {
                            measurementsData.Add(new KeyValueData()
                            {
                                name = "Сумма возврата",
                                value = Math.Abs(request.GetTypedColumnValue<float>("TrcAmountDeviation")).ToString().Replace(",00", "")
                            });
                        }

                        if(TrcDeviationInSquareMeters > 1 || TrcDeviationInSquareMeters < -1)
                        {
                            measurementsData.Add(new KeyValueData()
                            {
                                name = "Долевой взнос по договору",
                                value = request.GetTypedColumnValue<string>("TrcSharedContributionUnderContract").Replace(",00", "")
                            });

                            measurementsData.Add(new KeyValueData()
                            {
                                name = "Средняя стоимость кв. м по договору",
                                value = request.GetTypedColumnValue<string>("TrcAveragePricePerSquareMeterUnderContract").Replace(",00", "")
                            });

                            measurementsData.Add(new KeyValueData()
                            {
                                name = "Общая сумма после ПИБ",
                                value = request.GetTypedColumnValue<string>("TrcTotalAmountAfterPIB").Replace(",00", "")
                            });
                        }

                        response.measurements = new Measurement()
                        {
                            position = 3,
                            headInfo = "Расчет площади и суммы",
                            data = measurementsData.ToArray()
                        };

                        response.alert = new Alert()
                        {
                            position = 1,
                            headAl = "Заголовок алерта",
                            textAl = "Текст алерта",
                            color = "info"
                        };

                        response.prompt = new Prompt()
                        {
                            position = 2,
                            text = "Текстовая подсказка клиенту"
                        };
                    }

                    esq = new EntitySchemaQuery(this.UserConnection.EntitySchemaManager, "TrcRequestFile");

                    esq.AddAllSchemaColumns();

                    esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcRequest", request.PrimaryColumnValue));
                    esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcNotarialDocumentType", Guid.Parse("453249AA-F2C3-4F2B-AA78-0012CDB47F50")));

                    var file = esq.GetEntityCollection(this.UserConnection).FirstOrDefault();

                    if (file != null)
                    {
                        var ext = string.Empty;
                        var name = string.Empty;

                        var extArr = file.GetTypedColumnValue<string>("Name").Split('.');

                        if (extArr.Length > 1)
                        {
                            ext = extArr.Last();
                        }

                        name = extArr.First();

                        response.agreemDoc = new AgreemDoc()
                        {
                            position = 4,

                            docs = new Document()
                            {
                                name = name,
                                content = Convert.ToBase64String(file.GetColumnValue("Data") as byte[]),
                                type = ext
                            },
                            
                            consentCheck = new ConsentCheck()
                            {
                                name = "Согласовано",
                                content = request.GetTypedColumnValue<bool>("TrcAgreed") ? 0 : 1
                            },

                            comment = request.GetTypedColumnValue<bool>("TrcAgreed") ? 0 : 1,
                            fileAdd = request.GetTypedColumnValue<bool>("TrcAgreed") ? 0 : 1
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                response.Code = 500;
                response.Result = false;
                response.ReasonPhrase = ex.Message;
            }

            return response;
        }

        protected override void InitRequiredFields(List<string> requiredFields)
        {
            requiredFields.Add("contractId");
        }

        protected override void CheckRequiredFields(ContractsManualSettServiceRequestModel request, ContractsManualSettServiceResponseModel response)
        {
            base.CheckRequiredFields(request, response);
        }
    }
}
