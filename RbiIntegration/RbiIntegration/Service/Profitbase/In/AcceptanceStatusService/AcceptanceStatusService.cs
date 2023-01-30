using RbiIntegration.Service.BaseClasses;
using RbiIntegration.Service.Profitbase.In.AcceptanceStatusService.Model.Request;
using RbiIntegration.Service.Profitbase.In.AcceptanceStatusService.Model.Response;
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

namespace RbiIntegration.Service.Profitbase.In.AcceptanceStatusService
{
    /// <summary>
    /// Сервис получения текущего статуса приемки
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class AcceptanceStatusService : BaseRbiService<AcceptanceStatusServiceRequestModel, AcceptanceStatusServiceResponseModel>
    {
        public AcceptanceStatusService(UserConnection UserConnection) : base(UserConnection)
        {
            this.UserConnection = UserConnection;
        }

        protected override Guid GetIntegrationServiceId(AcceptanceStatusServiceRequestModel requestModel)
        {
            return CrmConstants.TrcIntegrationServices.AcceptanceStatus;
        }

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        protected override AcceptanceStatusServiceResponseModel ProcessBusinessLogic(AcceptanceStatusServiceRequestModel requestModel, AcceptanceStatusServiceResponseModel response)
        {
            Entity contract = null;
            Entity request = null;
            Entity product = null;
            Entity assignmentOfTimeSlot = null;
            Entity timeSlot = null;

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

                esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcRequestType", Guid.Parse("619B94D9-C3EC-4187-8774-1AA017B58BD8")));
                esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcService", Guid.Parse("643131D8-43D9-40BB-BB51-A70F2F3FCB7B")));
                esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.NotEqual, "TrcRequestStatus", Guid.Parse("3C08033C-1E3A-4C71-8EEE-2F8D8DCC7A2E")));
                esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcOpportunity", contract.GetTypedColumnValue<Guid>("TrcOpportunityId")));

                request = esq.GetEntityCollection(this.UserConnection).FirstOrDefault();

                if (request != null)
                {
                    esq = new EntitySchemaQuery(this.UserConnection.EntitySchemaManager, "Product");

                    esq.AddAllSchemaColumns();

                    esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Id", request.GetTypedColumnValue<Guid>("TrcObjectProfitbaseId")));

                    product = esq.GetEntityCollection(this.UserConnection).FirstOrDefault();

                    if (product != null)
                    {
                        int objectId;

                        if (int.TryParse(product.GetTypedColumnValue<string>("TrcObjectId"), out objectId))
                        {
                            response.objectId = objectId;
                        }
                    }

                    esq = new EntitySchemaQuery(this.UserConnection.EntitySchemaManager, "TrcAssignmentOfTimeSlots");

                    esq.AddAllSchemaColumns();

                    esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcRequest", request.PrimaryColumnValue));
                    esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcContact", request.GetTypedColumnValue<Guid>("TrcContactId")));

                    assignmentOfTimeSlot = esq.GetEntityCollection(this.UserConnection).FirstOrDefault();

                    if (assignmentOfTimeSlot != null)
                    {
                        if (assignmentOfTimeSlot.GetColumnValue("TrcAssignmentTimeslotId") != null)
                        {
                            response.acceptance = new Acceptance()
                            {
                                option = 0,
                                position = 3
                            };

                            esq = new EntitySchemaQuery(this.UserConnection.EntitySchemaManager, "TrcTimeslots");

                            esq.AddAllSchemaColumns();

                            esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Id", assignmentOfTimeSlot.GetTypedColumnValue<Guid>("TrcAssignmentTimeslotId")));

                            timeSlot = esq.GetEntityCollection(this.UserConnection).FirstOrDefault();

                            if (timeSlot != null)
                            {
                                response.acceptance.date = timeSlot.GetTypedColumnValue<DateTime>("TrcStartDate").ToString("dd-MM-yyyy");
                                response.acceptance.time = timeSlot.GetTypedColumnValue<DateTime>("TrcStartDate").ToString("hh-mm");
                            }
                        }
                        else
                        {
                            response.acceptance = new Acceptance()
                            {
                                option = 1,
                                position = 3
                            };
                        }
                    }

                    esq = new EntitySchemaQuery(this.UserConnection.EntitySchemaManager, "TrcRemarksInRequest");

                    esq.AddAllSchemaColumns();

                    esq.AddColumn("TrcRemarks.TrcName");

                    esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcRequest", request.PrimaryColumnValue));

                    var remarks = esq.GetEntityCollection(this.UserConnection);

                    if (remarks.Count > 0)
                    {
                        response.remarks = new Remarks()
                        {
                            position = 4,
                            headRem = "Замечания",
                            data = new List<Data>()
                        };

                        foreach (var item in remarks)
                        {
                            response.remarks.data.Add(new Data()
                            {
                                name = item.GetTypedColumnValue<string>("TrcRemarks_TrcName"),
                                date = item.GetTypedColumnValue<bool>("TrcStatus") ? null : item.GetTypedColumnValue<DateTime>("TrcEliminationDate").ToString("dd.MM.yyyy"),
                                state = new State()
                                {
                                    name = item.GetTypedColumnValue<bool>("TrcStatus") ? "Устранено" : "В работе",
                                    color = item.GetTypedColumnValue<bool>("TrcStatus") ? "#90EE90" : "#FFFACD"
                                }
                            });
                        }
                    }

                    response.alert = new Alert()
                    {
                        position = 1,
                        headAl = "Заголовок аллерта",
                        textAl = "Текст аллерта",
                        color = "info"
                    };

                    response.prompt = new Prompt()
                    {
                        position = 2,
                        text = "Текстовая подсказка клиенту"
                    };
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

        protected override void CheckRequiredFields(AcceptanceStatusServiceRequestModel request, AcceptanceStatusServiceResponseModel response)
        {
            base.CheckRequiredFields(request, response);
        }
    }
}
