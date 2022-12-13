using RbiIntegration.Service.BaseClasses;
using RbiIntegration.Service.In.Profitbase.CreateRequest.Model.Request;
using RbiIntegration.Service.In.Profitbase.CreateRequest.Model.Response;
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

namespace RbiIntegration.Service.In.Profitbase.CreateRequest
{
    /// <summary>
    /// Сервис создания заявки
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class CreateRequestService : BaseRbiService<CreateRequestServiceRequestModel, CreateRequestServiceResponseModel>
    {
        protected override Guid GetIntegrationServiceId()
        {
            return CrmConstants.TrcIntegrationServices.CreateRequest;
        }

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        protected override CreateRequestServiceResponseModel ProcessBusinessLogic(CreateRequestServiceRequestModel requestModel, CreateRequestServiceResponseModel response)
        {
            Entity contact = null;
            Entity request = null;

            try
            {
                contact = IntegrationServiceHelper.GetEntityByField(this.UserConnection, "Contact", "TrcProfitbaseLKId", requestModel.payload.user.userId);

                // Контакт найден
                UpdateContactTrcPersonalAccount(contact);
            }
            catch (Exception ex)
            {
                // Контакт не найден
                var reversedPhone = IntegrationServiceHelper.GetReversedPhone(requestModel.payload.user.phonenumber);

                var esq = new EntitySchemaQuery(this.UserConnection.EntitySchemaManager, "ContactCommunication");

                esq.AddColumn("Contact");

                esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "SearchNumber", reversedPhone));

                var entities = esq.GetEntityCollection(this.UserConnection);

                if (entities.Count == 1)
                {
                    contact = entities.First();

                    if (contact.GetTypedColumnValue<string>("GivenName") == requestModel.payload.user.name)
                    {
                        UpdateContactTrcPersonalAccount(contact);
                    }
                }
                else
                {
                    var fioArr = new string[]
                    {
                        requestModel.payload.user.surname,
                        requestModel.payload.user.name,
                        requestModel.payload.user.patronymic
                    };
          
                    contact = IntegrationServiceHelper.InsertEntityWithFields(this.UserConnection, "Contact", new Dictionary<string, object>()
                    {
                        { "Name", string.Join(" ", fioArr) },
                        { "GivenName", requestModel.payload.user.name },
                        { "TrcProfitbaseLKId", requestModel.payload.user.userId },
                        { "MobilePhone", requestModel.payload.user.phonenumber },
                        { "Email", requestModel.payload.user.email },
                        { "TrcPersonalAccount", true }
                    });
                }
            }

            // Ищем заявку
            try
            {
                request = IntegrationServiceHelper.GetEntityByField(this.UserConnection, "TrcRequest", "TrcRequestIdLK", requestModel.payload.documentId);

                // Заявка существует - возвращаем ошибку
                response.Result = false;
                response.Code = 500;
                response.ReasonPhrase = $"Заявка с идентификатором {requestModel.payload.documentId} уже существует";
            }
            catch (Exception)
            {
                // Заявки нет - все хорошо, создаем
                request = IntegrationServiceHelper.InsertEntityWithFields(this.UserConnection, "TrcRequest", new Dictionary<string, object>()
                        {
                            { "TrcRequestTypeId", Guid.Parse("512f0d01-99c1-4c1b-8ad2-9dcd4c56abc6") },
                            { "TrcContactId", contact.PrimaryColumnValue },
                            { "TrcRequestIdLK", requestModel.payload.documentId },
                            { "TrcRequestSourceId", Guid.Parse("b24e3d01-87be-4164-ae8e-559c6769d3c8") }
                        });

                response.Id = request.PrimaryColumnValue.ToString();

                var wrapper = new ServiceWrapper(this.UserConnection, "Enrichment");
                wrapper.SendRequest(request.PrimaryColumnValue);
            }

            return response;
        }

        protected void UpdateContactTrcPersonalAccount(Entity contact)
        {
            if (!contact.GetTypedColumnValue<bool>("TrcPersonalAccount"))
            {
                contact.SetColumnValue("TrcPersonalAccount", true);
                contact.Save(false);
            }
        }

        protected override void InitRequiredFields(List<string> requiredFields)
        {
            requiredFields.Add("type");
            requiredFields.Add("payload");
        }

        protected override void CheckRequiredFields(CreateRequestServiceRequestModel request, CreateRequestServiceResponseModel response)
        {
            base.CheckRequiredFields(request, response);

            if (response.Result)
            {
                if (string.IsNullOrEmpty(request.payload.documentId))
                {
                    response.ReasonPhrase = "Не заполнено обязательное поле payload.documentId";
                    response.Result = false;
                    response.Code = 500;
                    return;
                }

                if (string.IsNullOrEmpty(request.payload.workflowType))
                {
                    response.ReasonPhrase = "Не заполнено обязательное поле payload.workflowType";
                    response.Result = false;
                    response.Code = 500;
                    return;
                }

                if (request.payload.user == null)
                {
                    response.ReasonPhrase = "Не заполнено обязательное поле payload.user";
                    response.Result = false;
                    response.Code = 500;
                    return;
                }

                if (string.IsNullOrEmpty(request.payload.user.email))
                {
                    response.ReasonPhrase = "Не заполнено обязательное поле payload.user.email";
                    response.Result = false;
                    response.Code = 500;
                    return;
                }

                if (string.IsNullOrEmpty(request.payload.user.name))
                {
                    response.ReasonPhrase = "Не заполнено обязательное поле payload.user.name";
                    response.Result = false;
                    response.Code = 500;
                    return;
                }

                if (string.IsNullOrEmpty(request.payload.user.phonenumber))
                {
                    response.ReasonPhrase = "Не заполнено обязательное поле payload.user.phonenumber";
                    response.Result = false;
                    response.Code = 500;
                    return;
                }

                if (string.IsNullOrEmpty(request.payload.user.surname))
                {
                    response.ReasonPhrase = "Не заполнено обязательное поле payload.user.surname";
                    response.Result = false;
                    return;
                }

                if (string.IsNullOrEmpty(request.payload.user.userId))
                {
                    response.ReasonPhrase = "Не заполнено обязательное поле payload.user.userId";
                    response.Result = false;
                    response.Code = 500;
                    return;
                }

                if (request.payload.linkingDocument != null)
                {
                    if (string.IsNullOrEmpty(request.payload.linkingDocument.documentId))
                    {
                        response.ReasonPhrase = "Не заполнено обязательное поле payload.linkingDocument.documentId";
                        response.Result = false;
                        response.Code = 500;
                        return;
                    }

                    if (string.IsNullOrEmpty(request.payload.linkingDocument.workflowType))
                    {
                        response.ReasonPhrase = "Не заполнено обязательное поле payload.linkingDocument.workflowType";
                        response.Result = false;
                        response.Code = 500;
                        return;
                    }
                }
            }
        }
    }
}
