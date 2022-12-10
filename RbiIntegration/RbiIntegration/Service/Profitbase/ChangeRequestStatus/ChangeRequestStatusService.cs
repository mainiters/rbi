using RbiIntegration.Service.BaseClasses;
using RbiIntegration.Service.In.Profitbase.ChangeRequestStatus.Model.Request;
using RbiIntegration.Service.In.Profitbase.ChangeRequestStatus.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core.Entities;
using Terrasoft.Web.Common;
using static RbiIntegration.Service.BaseClasses.CrmConstants;

namespace RbiIntegration.Service.In.Profitbase.ChangeRequestStatus
{
    /// <summary>
    /// Сервис изменения статуса заявки
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class ChangeRequestStatusService : BaseRbiService<ChangeRequestStatusServiceRequestModel, ChangeRequestStatusServiceResponseModel>
    {
        protected override Guid GetIntegrationServiceId()
        {
            return CrmConstants.TrcIntegrationServices.ChangeRequestStatus;
        }

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        protected override ChangeRequestStatusServiceResponseModel ProcessBusinessLogic(ChangeRequestStatusServiceRequestModel requestModel, ChangeRequestStatusServiceResponseModel response)
        {
            try
            {
                if (requestModel.payload.previousStatus == "needAgr" && requestModel.payload.nextStatus == "sendServ")
                {
                    // Вызов GET запроса
                }
            }
            catch (Exception ex)
            {
                
            }

            return response;
        }

        protected override void InitRequiredFields(List<string> requiredFields)
        {
            requiredFields.Add("type");
            requiredFields.Add("payload");
        }

        protected override void CheckRequiredFields(ChangeRequestStatusServiceRequestModel request, ChangeRequestStatusServiceResponseModel response)
        {
            base.CheckRequiredFields(request, response);

            if (response.Result)
            {
                if (string.IsNullOrEmpty(request.payload.documentId))
                {
                    response.ReasonPhrase = "Не заполнено обязательное поле payload.documentId";
                    response.Result = false;
                    return;
                }

                if (string.IsNullOrEmpty(request.payload.workflowType))
                {
                    response.ReasonPhrase = "Не заполнено обязательное поле payload.workflowType";
                    response.Result = false;
                    return;
                }

                if (string.IsNullOrEmpty(request.payload.nextStatus))
                {
                    response.ReasonPhrase = "Не заполнено обязательное поле payload.nextStatus";
                    response.Result = false;
                    return;
                }

                if (string.IsNullOrEmpty(request.payload.previousStatus))
                {
                    response.ReasonPhrase = "Не заполнено обязательное поле payload.previousStatus";
                    response.Result = false;
                    return;
                }
            }
        }
    }
}
