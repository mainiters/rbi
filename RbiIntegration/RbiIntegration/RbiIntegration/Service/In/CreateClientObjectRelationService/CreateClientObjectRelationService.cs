using RbiIntegration.Service.BaseClasses;
using RbiIntegration.Service.In.CreateClientObjectRelationService.Model.Request;
using RbiIntegration.Service.In.CreateClientObjectRelationService.Model.Response;
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

namespace RbiIntegration.Service.In.CreateClientObjectRelationService
{
    /// <summary>
    /// Сервис создания связи клиента с помещением
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class CreateClientObjectRelationService : BaseService
    {
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public BaseResponse CreateClientObjectRelation(CreateClientObjectRelationServiceRequestModel requestModel)
        {
            DateTime requestInitDate = DateTime.Now;

            var res = new CreateClientObjectRelationServiceResponseModel();
            var request = IntegrationServiceHelper.ToJson(requestModel);

            var uid = string.Empty;
            var title = string.Empty;

            try
            {
                Entity contact = null;
                Entity contactRoleForObject = null;
                Entity product = null;
                try
                {
                    contact = IntegrationServiceHelper.FindLookupItem(this.UserConnection, "Contact", requestModel.TrcContactId, "Id", false, false).Entity;
                }
                catch (Exception ex)
                {
                    res.Result = false;
                    res.Code = 104002;
                    res.ReasonPhrase = $"Контакт с id {requestModel.TrcContactId} не найден";
                }

                if (contact != null)
                {
                    try
                    {
                        contactRoleForObject = IntegrationServiceHelper.FindLookupItem(this.UserConnection, "TrcContactRoleForObject", requestModel.TrcContactRoleForObjectId, "Id", false, false).Entity;
                    }
                    catch (Exception ex)
                    {
                        res.Result = false;
                        res.Code = 104003;
                        res.ReasonPhrase = $"Роль с id {requestModel.TrcContactRoleForObjectId} не найдена";
                    }
                }

                if (contactRoleForObject != null)
                {
                    try
                    {
                        product = IntegrationServiceHelper.FindLookupItem(this.UserConnection, "Product", requestModel.TrcObjectId, "Id", false, false).Entity;
                    }
                    catch (Exception ex)
                    {
                        res.Result = false;
                        res.Code = 104004;
                        res.ReasonPhrase = $"Объект с id {requestModel.TrcObjectId} не найден";
                    }
                }

                if (product != null)
                {
                    var connectionObjectWithContact = IntegrationServiceHelper.InsertEntityWithFields(this.UserConnection, "TrcConnectionObjectWithContact", new Dictionary<string, object>()
                    {
                        { "TrcObjectId", requestModel.TrcObjectId },
                        { "TrcContactId", requestModel.TrcContactId },
                        { "TrcCreatedByDomopult", true }
                    });

                    res.TrcConnectionObjectWithContactId = connectionObjectWithContact.PrimaryColumnValue.ToString();
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
