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
    public class RemoveClientProfileService : BaseService
    {
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public BaseResponse RemoveClientProfile(RemoveClientProfileServiceRequestModel requestModel)
        {
            DateTime requestInitDate = DateTime.Now;

            var res = new RemoveClientProfileServiceResponseModel();
            var request = IntegrationServiceHelper.ToJson(requestModel);

            var uid = string.Empty;
            var title = string.Empty;

            try
            {
                try
                {
                    var client = IntegrationServiceHelper.GetEntityByField(this.UserConnection, "Contact", "Id", requestModel.TrcContactId);

                    client.SetColumnValue("TrcDomopultDeletedOn", DateTime.Parse(requestModel.TrcDomopultDeletedOn));

                    client.Save(false);
                }
                catch (Exception ex)
                {
                    res.Result = false;
                    res.Code = 104002;
                    res.ReasonPhrase = $"Контакт с id {requestModel.TrcContactId} не найден";
                }

            }
            catch (Exception ex)
            {
                res.Code = 500;
                res.Message = "ERROR";
                res.Exception = ex.Message;
                res.StackTrace = ex.StackTrace;
                res.Result = false;
            }
            finally
            {
                IntegrationServiceHelper.Log(UserConnection, new IntegrationServiceParams() { Id = TrcIntegrationServices.CreateClientObjectRelation }, requestInitDate, title, uid, res.Exception, request, IntegrationServiceHelper.ToJson(res));
            }

            return res;
        }

    }
}
