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
    /// Сервис получения списка доступных договоров
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class ContractsManualSettService : BaseRbiService<ContractsManualSettServiceRequestModel, ContractsManualSettServiceResponseModel>
    {
        public ContractsManualSettService(UserConnection UserConnection) : base(UserConnection)
        {
            this.UserConnection = UserConnection;
        }

        protected override Guid GetIntegrationServiceId()
        {
            return CrmConstants.TrcIntegrationServices.ContractsInfo;
        }

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        protected override ContractsManualSettServiceResponseModel ProcessBusinessLogic(ContractsManualSettServiceRequestModel requestModel, ContractsManualSettServiceResponseModel response)
        {
            Entity contract = null;

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
