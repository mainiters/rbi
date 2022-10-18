using RbiIntegration.Service.BaseClasses;
using RbiIntegration.Service.In.AddClientObjectService.Model;
using RbiIntegration.Service.In.AddClientObjectService.Model.Request;
using RbiIntegration.Service.In.AddClientObjectService.Model.Response;
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
using RbiIntegration.Service.BaseClasses.Extensions;

namespace RbiIntegration.Service.In.AddClientObjectService
{
    /// <summary>
    /// Сервис добавления помещения клиенту
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class AddClientObjectService : BaseService
    {
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public BaseResponse AddClientObject(AddClientObjectServiceRequestModel requestModel)
        {
            DateTime requestInitDate = DateTime.Now;

            var res = new AddClientObjectServiceResponseModel();
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
                    contact = IntegrationServiceHelper.GetEntityByField(this.UserConnection, "Contact", "Id", requestModel.TrcContactId);
                }
                catch
                {
                }

                if (contact == null)
                {
                    if (requestModel.Phones == null || requestModel.Phones.Length < 1 || requestModel.Phones.Count(e => e.Basic) < 1)
                    {
                        throw new Exception("В запросе отстутствует основной номер телефона");
                    }

                    var phone = requestModel.Phones.First(e => e.Basic).Phone;

                    var reversedPhone = IntegrationServiceHelper.GetReversedPhone(phone);

                    var esq = new EntitySchemaQuery(this.UserConnection.EntitySchemaManager, "ContactCommunication");

                    esq.AddColumn("Contact");

                    esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "SearchNumber", reversedPhone));

                    var entities = esq.GetEntityCollection(this.UserConnection);

                    if (entities.Count < 1)
                    {
                        res.Result = false;
                        res.Code = 104001;
                        res.ReasonPhrase = $"Контакт с номером {phone} не найден";
                    }
                    else if (entities.Count > 1)
                    {
                        res.Result = false;
                        res.Code = 104001;
                        res.ReasonPhrase = $"Найдено более одного контакта с номером {phone}";
                    }
                    else
                    {
                        contact = entities.First();
                    }
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
                        product = IntegrationServiceHelper.FindLookupItem(this.UserConnection, "Product", requestModel.ProductId, "Id", false, false).Entity;
                    }
                    catch (Exception ex)
                    {
                        res.Result = false;
                        res.Code = 104004;
                        res.ReasonPhrase = $"Объект с id {requestModel.ProductId} не найден";
                    }
                }

                if (product != null)
                {
                    var connectionObjectWithContact = IntegrationServiceHelper.InsertEntityWithFields(this.UserConnection, "TrcConnectionObjectWithContact", new Dictionary<string, object>()
                    {
                        { "TrcObjectId", requestModel.ProductId },
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
                IntegrationServiceHelper.Log(UserConnection, new IntegrationServiceParams() { Id = TrcIntegrationServices.CheckClientExisting }, requestInitDate, title, uid, res.Exception, request, IntegrationServiceHelper.ToJson(res));
            }

            return res;
        }
    }
}
