using RbiIntegration.Service.BaseClasses;
using RbiIntegration.Service.Profitbase.In.ContractsAssignmentOfDaySlotsService.Model.Request;
using RbiIntegration.Service.Profitbase.In.ContractsAssignmentOfDaySlotsService.Model.Response;
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

namespace RbiIntegration.Service.Profitbase.In.ContractsAssignmentOfDaySlotsService
{
    /// <summary>
    /// Сервис получения списка доступных дат
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class ContractsAssignmentOfDaySlotsService : BaseRbiService<ContractsAssignmentOfDaySlotsServiceRequestModel, ContractsAssignmentOfDaySlotsServiceResponseModel>
    {
        public ContractsAssignmentOfDaySlotsService(UserConnection UserConnection) : base(UserConnection)
        {
            this.UserConnection = UserConnection;
        }

        protected override Guid GetIntegrationServiceId(ContractsAssignmentOfDaySlotsServiceRequestModel requestModel)
        {
            return CrmConstants.TrcIntegrationServices.ContractsInfo;
        }

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        protected override ContractsAssignmentOfDaySlotsServiceResponseModel ProcessBusinessLogic(ContractsAssignmentOfDaySlotsServiceRequestModel requestModel, ContractsAssignmentOfDaySlotsServiceResponseModel response)
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

                response.date = new string[] 
                { 
                    timeSlot.GetTypedColumnValue<DateTime>("TrcDay").ToString("dd-MM-yyyy")
                };
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

        protected override void CheckRequiredFields(ContractsAssignmentOfDaySlotsServiceRequestModel request, ContractsAssignmentOfDaySlotsServiceResponseModel response)
        {
            base.CheckRequiredFields(request, response);
        }
    }
}
