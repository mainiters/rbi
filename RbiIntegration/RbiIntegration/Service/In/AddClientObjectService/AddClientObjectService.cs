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
    public class AddClientObjectService : BaseRbiService<AddClientObjectServiceRequestModel, AddClientObjectServiceResponseModel>
    {
        protected override Guid GetIntegrationServiceId()
        {
            return CrmConstants.TrcIntegrationServices.AddClientObject;
        }

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        protected override AddClientObjectServiceResponseModel ProcessBusinessLogic(AddClientObjectServiceRequestModel requestModel, AddClientObjectServiceResponseModel response)
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
                if (requestModel.Phones == null || requestModel.Phones.Length < 1 || requestModel.Phones.Count(e => e.Basic == true) < 1)
                {
                    throw new Exception("В запросе отсутствует основной номер телефона");
                }

                var phone = requestModel.Phones.First(e => e.Basic == true).Phone;

                var reversedPhone = IntegrationServiceHelper.GetReversedPhone(phone);

                var esq = new EntitySchemaQuery(this.UserConnection.EntitySchemaManager, "ContactCommunication");

                esq.AddColumn("Contact");

                esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "SearchNumber", reversedPhone));

                var entities = esq.GetEntityCollection(this.UserConnection);

                if (entities.Count < 1)
                {
                    response.Result = false;
                    response.Code = 104001;
                    response.ReasonPhrase = $"Контакт с номером {phone} не найден";
                }
                else if (entities.Count > 1)
                {
                    response.Result = false;
                    response.Code = 104001;
                    response.ReasonPhrase = $"Найдено более одного контакта с номером {phone}";
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
                    response.Result = false;
                    response.Code = 104003;
                    response.ReasonPhrase = $"Роль с id {requestModel.TrcContactRoleForObjectId} не найдена";
                }
            }

            if (contactRoleForObject != null)
            {
                try
                {
                    product = IntegrationServiceHelper.FindLookupItem(this.UserConnection, "Product", requestModel.ProductId, "Id", false, false).Entity;

                    product.SetColumnValue("TrcPersonalAccount", requestModel.TrcPersonalAccount);
                    product.Save();
                }
                catch (Exception ex)
                {
                    response.Result = false;
                    response.Code = 104004;
                    response.ReasonPhrase = $"Объект с id {requestModel.ProductId} не найден";
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

                response.TrcConnectionObjectWithContactId = connectionObjectWithContact.PrimaryColumnValue.ToString();
            }

            return response;
        }

        protected override void InitRequiredFields(List<string> requiredFields)
        {
            requiredFields.Add("TrcContactId");
            requiredFields.Add("Phones");
            requiredFields.Add("ProductId");
            requiredFields.Add("TrcContactRoleForObjectId");
            requiredFields.Add("TrcPersonalAccount");
        }

        protected override void CheckRequiredFields(AddClientObjectServiceRequestModel request, AddClientObjectServiceResponseModel response)
        {
            base.CheckRequiredFields(request, response);

            if (response.Result)
            {
                foreach (var item in request.Phones)
                {
                    if (item.Basic == null)
                    {
                        response.ReasonPhrase = $"Обязательное поле Basic не заполнено";
                        response.Code = 304001;
                        response.Result = false;
                        return;
                    }

                    if (string.IsNullOrEmpty(item.Phone))
                    {
                        response.ReasonPhrase = $"Обязательное поле Phone не заполнено";
                        response.Code = 304001;
                        response.Result = false;
                        return;
                    }
                }
            }
        }
    }
}
