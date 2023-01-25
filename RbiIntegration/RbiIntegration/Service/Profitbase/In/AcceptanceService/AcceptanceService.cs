using RbiIntegration.Service.BaseClasses;
using RbiIntegration.Service.Profitbase.In.AcceptanceService.Model.Request;
using RbiIntegration.Service.Profitbase.In.AcceptanceService.Model.Response;
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

namespace RbiIntegration.Service.Profitbase.In.AcceptanceService
{
    /// <summary>
    /// Согласование
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class AcceptanceService : BaseRbiService<AcceptanceServiceRequestModel, AcceptanceServiceResponseModel>
    {
        public AcceptanceService(UserConnection UserConnection) : base(UserConnection)
        {
            this.UserConnection = UserConnection;
        }

        protected override Guid GetIntegrationServiceId(AcceptanceServiceRequestModel requestModel)
        {
            if (requestModel.type == "acceptanceDate")
            {
                return CrmConstants.TrcIntegrationServices.AcceptanceDate;
            }

            return CrmConstants.TrcIntegrationServices.AcceptanceDocument;
        }

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        protected override AcceptanceServiceResponseModel ProcessBusinessLogic(AcceptanceServiceRequestModel requestModel, AcceptanceServiceResponseModel response)
        {
            Entity contract = null;
            Entity request = null;

            try
            {
                if (requestModel.type == "acceptanceDate")
                {
                    contract = IntegrationServiceHelper.GetEntityByField(this.UserConnection, "Contract", "Id", requestModel.payload.contractId);

                    var esq = new EntitySchemaQuery(this.UserConnection.EntitySchemaManager, "TrcRequest");

                    esq.AddAllSchemaColumns();

                    esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcOpportunity", contract.GetTypedColumnValue<Guid>("TrcOpportunityId")));
                    esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcRequestType", Guid.Parse("619b94d9-c3ec-4187-8774-1aa017b58bd8")));
                    esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.NotEqual, "TrcRequestStatus", Guid.Parse("19ECC014-1CF2-412E-B918-9D898E04AB1D")));
                    esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.NotEqual, "TrcRequestStatus", Guid.Parse("0743199E-CDC5-493F-88FB-BF5777720814")));

                    request = esq.GetEntityCollection(this.UserConnection).FirstOrDefault();

                    if (request != null)
                    {
                        request.SetColumnValue("TrcAcceptanceDecisionId", requestModel.payload.acceptance == 0 
                            ? Guid.Parse("EFC2B317-085E-4656-AB8C-7A671E3653B3") 
                            : Guid.Parse("D297BA1D-54C6-484F-B333-64D5B8FFCBAB"));

                        esq = new EntitySchemaQuery(this.UserConnection.EntitySchemaManager, "TrcAssignmentOfTimeSlots");

                        esq.AddAllSchemaColumns();

                        esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcRequest", request.PrimaryColumnValue));

                        var assignmentOfTimeSlots = esq.GetEntityCollection(this.UserConnection);
                        var assignmentOfTimeSlot = assignmentOfTimeSlots.FirstOrDefault();

                        if (requestModel.payload.acceptance == 1)
                        {
                            request.SetColumnValue("TrcAcceptanceDate", DateTime.Parse(requestModel.payload.date));
                            request.SetColumnValue("TrcAcceptanceTime", DateTime.Parse(requestModel.payload.time).TimeOfDay);

                            if (assignmentOfTimeSlot.GetColumnValue("TrcAssignmentTimeslotId") == null)
                            {
                                esq = new EntitySchemaQuery(this.UserConnection.EntitySchemaManager, "TrcTimeslots");

                                esq.AddAllSchemaColumns();

                                esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcObject", assignmentOfTimeSlot.GetTypedColumnValue<Guid>("TrcObjectId")));
                                esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcCheckInShedule", assignmentOfTimeSlot.GetTypedColumnValue<Guid>("TrcCheckInSheduleId")));

                                var timeSlots = esq.GetEntityCollection(this.UserConnection);

                                if (timeSlots.Count > 0)
                                {
                                    var timeSlot = timeSlots.Where(e => e.GetTypedColumnValue<DateTime>("TrcStartDate").Date == DateTime.Parse(requestModel.payload.date).Date
                                                    && e.GetTypedColumnValue<DateTime>("TrcStartDate").TimeOfDay == DateTime.Parse(requestModel.payload.date).TimeOfDay).FirstOrDefault();

                                    if (timeSlot != null)
                                    {
                                        foreach (var item in assignmentOfTimeSlots)
                                        {
                                            item.SetColumnValue("TrcAssignmentTimeslotId", timeSlot.PrimaryColumnValue);
                                            item.Save(false);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            var TrcAssignmentTimeslotId = assignmentOfTimeSlot.GetTypedColumnValue<string>("TrcAssignmentTimeslotId");

                            if (!string.IsNullOrEmpty(TrcAssignmentTimeslotId))
                            {
                                foreach (var item in assignmentOfTimeSlots)
                                {
                                    item.SetColumnValue("TrcAssignmentTimeslotId", string.Empty);
                                    item.Save(false);
                                }

                                esq = new EntitySchemaQuery(this.UserConnection.EntitySchemaManager, "TrcTimeslots");

                                esq.AddAllSchemaColumns();

                                esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Id", TrcAssignmentTimeslotId));

                                var timeSlot = esq.GetEntityCollection(this.UserConnection).FirstOrDefault();

                                if (timeSlot != null)
                                {
                                    request.SetColumnValue("TrcAcceptanceDate", timeSlot.GetTypedColumnValue<DateTime>("TrcStartDate").Date);
                                    request.SetColumnValue("TrcAcceptanceTime", timeSlot.GetTypedColumnValue<DateTime>("TrcStartDate").TimeOfDay);
                                    request.Save(false);
                                }
                            }
                            else
                            {
                                request.SetColumnValue("TrcAcceptanceDate", assignmentOfTimeSlot.GetTypedColumnValue<DateTime>("TrcStartDate"));
                                request.Save(false);

                                foreach (var item in assignmentOfTimeSlots)
                                {
                                    item.SetColumnValue("TrcDay", string.Empty);
                                    item.Save(false);
                                }
                            }
                        }

                        request.Save(false);
                    }
                }
                else if(requestModel.type == "acceptanceDocument")
                {
                    contract = IntegrationServiceHelper.GetEntityByField(this.UserConnection, "Contract", "Id", requestModel.payload.contractId);

                    var esq = new EntitySchemaQuery(this.UserConnection.EntitySchemaManager, "TrcRequest");

                    esq.AddAllSchemaColumns();

                    esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcOpportunity", contract.GetTypedColumnValue<Guid>("TrcOpportunityId")));
                    esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcRequestType", Guid.Parse("512F0D01-99C1-4C1B-8AD2-9DCD4C56ABC6")));
                    esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcService", Guid.Parse("82983928-3428-4201-B44F-E181F711873D")));
                    esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.NotEqual, "TrcRequestStatus", Guid.Parse("19ECC014-1CF2-412E-B918-9D898E04AB1D")));
                    esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.NotEqual, "TrcRequestStatus", Guid.Parse("0743199E-CDC5-493F-88FB-BF5777720814")));

                    request = esq.GetEntityCollection(this.UserConnection).FirstOrDefault();

                    if (request != null)
                    {
                        request.SetColumnValue("TrcAgreed", requestModel.payload.acceptance == 1);

                        if (requestModel.payload.acceptance == 0)
                        {
                            request.SetColumnValue("TrcRejectionReason", requestModel.payload.comment);
                        }

                        request.Save(false);
                    }
                }
                else
                {
                    throw new Exception($"Недопустимое значение {requestModel.type} для параметра type");
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
            requiredFields.Add("type");
            requiredFields.Add("payload");
        }

        protected override void CheckRequiredFields(AcceptanceServiceRequestModel request, AcceptanceServiceResponseModel response)
        {
            base.CheckRequiredFields(request, response);
        }
    }
}
