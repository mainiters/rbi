using RbiIntegration.Service.BaseClasses;
using RbiIntegration.Service.Profitbase.In.ContractsInfoService.Model.Request;
using RbiIntegration.Service.Profitbase.In.ContractsInfoService.Model.Response;
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

namespace RbiIntegration.Service.Profitbase.In.ContractsInfoService
{
    /// <summary>
    /// Сервис получения списка доступных договоров
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class ContractsInfoService : BaseRbiService<ContractsInfoServiceRequestModel, ContractsInfoServiceResponseModel>
    {
        public ContractsInfoService(UserConnection UserConnection) : base(UserConnection)
        {
            this.UserConnection = UserConnection;
        }

        protected override Guid GetIntegrationServiceId(ContractsInfoServiceRequestModel requestModel)
        {
            return CrmConstants.TrcIntegrationServices.ContractsInfo;
        }

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        protected override ContractsInfoServiceResponseModel ProcessBusinessLogic(ContractsInfoServiceRequestModel requestModel, ContractsInfoServiceResponseModel response)
        {
            Entity contract = null;
            Entity request = null;
            Entity opportunity = null;

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

                esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcContractRequest", requestModel.contractId));

                request = esq.GetEntityCollection(this.UserConnection).FirstOrDefault();

                if (request != null && request.GetTypedColumnValue<Guid>("TrcRequestTypeId") == Guid.Parse("512F0D01-99C1-4C1B-8AD2-9DCD4C56ABC6"))
                {
                    if (request.GetTypedColumnValue<Guid>("TrcServiceId") == Guid.Parse("82983928-3428-4201-B44F-E181F711873D")
                        && request.GetTypedColumnValue<Guid>("TrcRequestStatusId") != Guid.Parse("19ECC014-1CF2-412E-B918-9D898E04AB1D")
                        && request.GetTypedColumnValue<Guid>("TrcRequestStatusId") != Guid.Parse("0743199E-CDC5-493F-88FB-BF5777720814"))
                    {
                        response.availMutSett = 1;
                    }
                }

                esq = new EntitySchemaQuery(this.UserConnection.EntitySchemaManager, "TrcRequest");

                esq.AddAllSchemaColumns();

                esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcRequestType", Guid.Parse("619B94D9-C3EC-4187-8774-1AA017B58BD8")));
                esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.NotEqual, "TrcRequestStatus", Guid.Parse("19ECC014-1CF2-412E-B918-9D898E04AB1D")));
                esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.NotEqual, "TrcRequestStatus", Guid.Parse("0743199E-CDC5-493F-88FB-BF5777720814")));
                esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcOpportunity", contract.GetTypedColumnValue<Guid>("TrcOpportunityId")));

                request = esq.GetEntityCollection(this.UserConnection).FirstOrDefault();

                if (request != null)
                {
                    response.availAcceptEstate = 1;
                }

                esq = new EntitySchemaQuery(this.UserConnection.EntitySchemaManager, "Opportunity");

                esq.AddAllSchemaColumns();

                esq.AddColumn("TrcObject.TrcObjectId");

                esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Id", contract.GetTypedColumnValue<Guid>("TrcOpportunityId")));

                opportunity = esq.GetEntityCollection(this.UserConnection).FirstOrDefault();

                if (opportunity != null && opportunity.GetColumnValue("TrcFinalFreeCalculationId") != null && opportunity.GetTypedColumnValue<bool>("TrcContractSigned"))
                {
                    response.availPaymentSchedule = 1;
                }

                var contractDate = contract.GetTypedColumnValue<DateTime>("TrcContractDate");

                var values = new List<KeyValueData>
                {
                    new KeyValueData()
                    {
                        name = "Номер договора",
                        value = contract.GetTypedColumnValue<string>("Number")
                    },
                    new KeyValueData()
                    {
                        name = "Тип договора",
                        value = contract.GetTypedColumnValue<string>("TrcType_Name")
                    },
                    new KeyValueData()
                    {
                        name = "Дата договора",
                        value = $"{contractDate.Day} {GetMonthName(contractDate.Month)} {contractDate.Year} г"
                    },
                    new KeyValueData()
                    {
                        name = "Состояние договора",
                        value = contract.GetTypedColumnValue<string>("State_Name")
                    },
                    new KeyValueData()
                    {
                        name = "Стоимость договора",
                        value = contract.GetTypedColumnValue<string>("TrcContractPrice").Replace(",00", "")
                    }
                };

                if (contract.GetTypedColumnValue<Guid>("TrcTypeId") == Guid.Parse("7923DF2F-B150-4BE4-93B1-42B7AC2167D7"))
                {
                    values.Add(new KeyValueData()
                    {
                        name = "Родительский договор",
                        value = contract.GetTypedColumnValue<string>("Parent_Number")
                    });
                }

                esq = new EntitySchemaQuery(this.UserConnection.EntitySchemaManager, "OpportunityContact");

                esq.AddAllSchemaColumns();
                
                esq.AddColumn("Contact.Name");
                esq.AddColumn("Role.Name");
                
                esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Opportunity", contract.GetTypedColumnValue<Guid>("TrcOpportunityId")));

                var opportunityContacts = esq.GetEntityCollection(this.UserConnection);

                var buyers = new List<Buyer>();

                if (opportunityContacts != null && opportunityContacts.Count > 0)
                {
                    foreach (var item in opportunityContacts)
                    {
                        buyers.Add(new Buyer()
                        {
                            name = item.GetTypedColumnValue<string>("Contact_Name"),
                            role = item.GetTypedColumnValue<string>("Role_Name"),
                            share = item.GetTypedColumnValue<string>("TrcOwnershipShare")
                        });
                    }
                }

                response.general = new General()
                {
                    position = 1,
                    headInfo = "Данные договора",
                    data = values.ToArray(),
                    buyers = buyers.ToArray()
                };

                if (opportunity.GetColumnValue("TrcObject_TrcObjectId") != null)
                {
                    int val;

                    if (int.TryParse(opportunity.GetTypedColumnValue<string>("TrcObject_TrcObjectId"), out val))
                    {
                        response.objectId = val;
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

        protected string GetMonthName(int number)
        {
            switch (number)
            {
                case 1:
                    return "января";
                case 2:
                    return "февраля";
                case 3:
                    return "марта";
                case 4:
                    return "апреля";
                case 5:
                    return "мая";
                case 6:
                    return "июня";
                case 7:
                    return "июля";
                case 8:
                    return "августа";
                case 9:
                    return "сентября";
                case 10:
                    return "октября";
                case 11:
                    return "ноября";
                case 12:
                    return "декабря";
                default:
                    return string.Empty;
            }
        }

        protected override void InitRequiredFields(List<string> requiredFields)
        {
            requiredFields.Add("contractId");
        }

        protected override void CheckRequiredFields(ContractsInfoServiceRequestModel request, ContractsInfoServiceResponseModel response)
        {
            base.CheckRequiredFields(request, response);
        }
    }
}
