using RbiIntegration.Service.BaseClasses;
using RbiIntegration.Service.In.RemoveClientProfileService.Model.Request;
using RbiIntegration.Service.In.RemoveClientProfileService.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.DB;
using Terrasoft.Core.Entities;
using Terrasoft.Web.Common;
using static RbiIntegration.Service.BaseClasses.CrmConstants;

namespace RbiIntegration.Service.In.RemoveClientProfileService
{
    /// <summary>
    /// Сервис удаления профиля клиента
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class RemoveClientProfileService : BaseRbiService<RemoveClientProfileServiceRequestModel, RemoveClientProfileServiceResponseModel>
    {
        public RemoveClientProfileService(UserConnection UserConnection) : base(UserConnection)
        {

        }

        protected override Guid GetIntegrationServiceId(RemoveClientProfileServiceRequestModel requestModel)
        {
            return CrmConstants.TrcIntegrationServices.RemoveClientProfile;
        }

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        protected override RemoveClientProfileServiceResponseModel ProcessBusinessLogic(RemoveClientProfileServiceRequestModel requestModel, RemoveClientProfileServiceResponseModel response)
        {
            try
            {
                var client = IntegrationServiceHelper.GetEntityByField(this.UserConnection, "Contact", "Id", requestModel.TrcContactId);

                client.SetColumnValue("TrcDomopultDeletedOn", DateTime.Parse(requestModel.TrcDomopultDeletedOn));

                client.Save(false);
            }
            catch (Exception ex)
            {
                response.Result = false;
                response.Code = 104002;
                response.ReasonPhrase = $"Контакт с id {requestModel.TrcContactId} не найден";
            }

            return response;
        }

        protected override void InitRequiredFields(List<string> requiredFields)
        {
            requiredFields.Add("TrcContactId");
            requiredFields.Add("TrcDomopultDeletedOn");
        }
    }
}
