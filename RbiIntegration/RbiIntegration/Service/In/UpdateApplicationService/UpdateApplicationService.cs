using RbiIntegration.Service.BaseClasses;
using RbiIntegration.Service.In.UpdateApplicationService.Model.Request;
using RbiIntegration.Service.In.UpdateApplicationService.Model.Response;
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

namespace RbiIntegration.Service.In.UpdateApplicationService
{
    /// <summary>
    /// Сервис обновления заявки
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class UpdateApplicationService : BaseRbiService<UpdateApplicationServiceRequestModel, UpdateApplicationServiceResponseModel>
    {
        public UpdateApplicationService(UserConnection UserConnection) : base(UserConnection)
        {

        }

        protected override Guid GetIntegrationServiceId()
        {
            return CrmConstants.TrcIntegrationServices.UpdateApplication;
        }

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        protected override UpdateApplicationServiceResponseModel ProcessBusinessLogic(UpdateApplicationServiceRequestModel requestModel, UpdateApplicationServiceResponseModel response)
        {
            Entity service = null;
            Entity status = null;
            Entity request = null;

            try
            {
                service = IntegrationServiceHelper.GetEntityByField(this.UserConnection, "TrcService", "Id", requestModel.TrcServiceId);
            }
            catch (Exception ex)
            {
                response.Result = false;
                response.Code = 304042;
                response.ReasonPhrase = $"Услуга с id {requestModel.TrcServiceId} не найдена";
                return response;
            }

            try
            {
                status = IntegrationServiceHelper.GetEntityByField(this.UserConnection, "TrcRequestStatus", "Id", requestModel.TrcRequestStatusId);
            }
            catch (Exception ex)
            {
                response.Result = false;
                response.Code = 304002;
                response.ReasonPhrase = $"Статус заявки с id {requestModel.TrcRequestStatusId} не найден";
                return response;
            }

            try
            {
                request = IntegrationServiceHelper.GetEntityByField(this.UserConnection, "TrcRequest", "Id", requestModel.TrcRequestId);
            }
            catch (Exception ex)
            {
                response.Result = false;
                response.Code = 304401;
                response.ReasonPhrase = $"Заявка с id {requestModel.TrcRequestId} не найдена";
                return response;
            }

            IntegrationServiceHelper.InsertOrUpdateEntity(this.UserConnection, "TrcRequest", "Id", request.PrimaryColumnValue, new Dictionary<string, object>()
            {
                { "TrcServiceId", service.PrimaryColumnValue },
                { "TrcRequestStatusId", status.PrimaryColumnValue },
                { "TrcName", requestModel.TrcName },
                { "TrcDescription", requestModel.TrcDescription }
            });

            return response;
        }

        protected override void InitRequiredFields(List<string> requiredFields)
        {
            requiredFields.Add("TrcServiceId");
            requiredFields.Add("TrcRequestId");
            requiredFields.Add("TrcRequestStatusId");
            requiredFields.Add("TrcName");
        }
    }
}
