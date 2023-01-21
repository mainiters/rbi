using RbiIntegration.Service.BaseClasses;
using RbiIntegration.Service.Profitbase.In.ContractsAssignmentOfTimeSlotsService.Model.Request;
using RbiIntegration.Service.Profitbase.In.ContractsAssignmentOfTimeSlotsService.Model.Response;
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

namespace RbiIntegration.Service.Profitbase.In.ContractsAssignmentOfTimeSlotsService
{
    /// <summary>
    /// Сервис получения списка доступных тайм-слотов
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class ContractsAssignmentOfTimeSlotsService : BaseRbiService<ContractsAssignmentOfTimeSlotsServiceRequestModel, ContractsAssignmentOfTimeSlotsServiceResponseModel>
    {
        public ContractsAssignmentOfTimeSlotsService(UserConnection UserConnection) : base(UserConnection)
        {
            this.UserConnection = UserConnection;
        }

        protected override Guid GetIntegrationServiceId(ContractsAssignmentOfTimeSlotsServiceRequestModel requestModel)
        {
            return CrmConstants.TrcIntegrationServices.ContractsInfo;
        }

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        protected override ContractsAssignmentOfTimeSlotsServiceResponseModel ProcessBusinessLogic(ContractsAssignmentOfTimeSlotsServiceRequestModel requestModel, ContractsAssignmentOfTimeSlotsServiceResponseModel response)
        {
            Entity contract = null;

            try
            {
                contract = IntegrationServiceHelper.GetEntityByField(this.UserConnection, "Contract", "Id", requestModel.contractId);

                var esq = new EntitySchemaQuery(this.UserConnection.EntitySchemaManager, "TrcAssignmentOfTimeSlots");

                esq.AddAllSchemaColumns();

                esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcOpportunity", contract.GetTypedColumnValue<Guid>("TrcOpportunityId")));
                esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcOpportunity", contract.GetTypedColumnValue<Guid>("TrcOpportunityId")));
                esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcContact", contract.GetTypedColumnValue<Guid>("TrcContactId")));

                var timeSlot = esq.GetEntityCollection(this.UserConnection).FirstOrDefault();

                if (timeSlot.GetTypedColumnValue<DateTime>("TrcDay").Date != DateTime.Parse(requestModel.date).Date)
                {
                    throw new Exception($"Дата {requestModel.date} не найдена");
                }

                esq = new EntitySchemaQuery(this.UserConnection.EntitySchemaManager, "TrcTimeslots");

                esq.AddAllSchemaColumns();

                esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcObject", timeSlot.GetTypedColumnValue<Guid>("TrcObjectId")));
                esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcCheckInShedule", timeSlot.GetTypedColumnValue<Guid>("TrcCheckInSheduleId")));
                esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcTimeSlotStartDate", DateTime.Parse(requestModel.date).Date));

                var times = esq.GetEntityCollection(this.UserConnection);

                var timesList = new List<string>();

                foreach (var item in times)
                {
                    timesList.Add(item.GetTypedColumnValue<DateTime>("TrcTimeSlotStartTime").ToString("HH:mm"));
                }

                response.time = timesList.ToArray();
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
            requiredFields.Add("date");
        }

        protected override void CheckRequiredFields(ContractsAssignmentOfTimeSlotsServiceRequestModel request, ContractsAssignmentOfTimeSlotsServiceResponseModel response)
        {
            base.CheckRequiredFields(request, response);
        }
    }
}
