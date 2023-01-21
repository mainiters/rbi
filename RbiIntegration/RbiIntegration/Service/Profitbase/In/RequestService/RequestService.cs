using RbiIntegration.Service.BaseClasses;
using RbiIntegration.Service.Profitbase.In.RequestService.Model.Request;
using RbiIntegration.Service.Profitbase.In.RequestService.Model.Response;
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

namespace RbiIntegration.Service.Profitbase.In.RequestService
{
    /// <summary>
    /// Сервис раоботы с заявками
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class RequestService : BaseRbiService<RequestServiceRequestModel, RequestServiceResponseModel>
    {
        public RequestService(UserConnection UserConnection) : base(UserConnection)
        {
            this.UserConnection = UserConnection;
        }

        protected override Guid GetIntegrationServiceId(RequestServiceRequestModel requestModel)
        {
            return CrmConstants.TrcIntegrationServices.Request;
        }

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        protected override RequestServiceResponseModel ProcessBusinessLogic(RequestServiceRequestModel requestModel, RequestServiceResponseModel response)
        {
            Entity contact = null;
            Entity request = null;

            if(requestModel.type == "documentChangeStatus")
            {
                try
                {
                    if (requestModel.payload.nextStatus == "sendServ")
                    {
                        string documentId = string.IsNullOrEmpty(requestModel.payload.documentid) ? requestModel.payload.documentId : requestModel.payload.documentid;

                        request = IntegrationServiceHelper.GetEntityByField(this.UserConnection, "TrcRequest", "TrcRequestIdLK", documentId);

                        var wrapper = new ServiceWrapper(this.UserConnection, "Enrichment");
                        wrapper.SendRequest(request.PrimaryColumnValue.ToString());
                    }
                }
                catch (Exception ex)
                {
                    response.Code = 500;
                    response.Result = false;
                    response.ReasonPhrase = ex.Message;
                }
            }
            else if(requestModel.type == "documentCreated")
            {
                try
                {
                    contact = IntegrationServiceHelper.GetEntityByField(this.UserConnection, "Contact", "TrcProfitbaseLKId", requestModel.payload.user.userId);

                    // Контакт найден
                    UpdateContactTrcPersonalAccount(contact, requestModel);
                }
                catch (Exception ex)
                {
                    // Контакт не найден
                    var reversedPhone = IntegrationServiceHelper.GetReversedPhone(requestModel.payload.user.phonenumber);

                    var esq = new EntitySchemaQuery(this.UserConnection.EntitySchemaManager, "ContactCommunication");

                    esq.AddColumn("Contact");

                    esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "SearchNumber", reversedPhone));

                    var entities = esq.GetEntityCollection(this.UserConnection);

                    if(entities.Count > 1)
                    {
                        response.Code = 500;
                        response.Result = false;
                        response.ReasonPhrase = $"Для номера телефона {requestModel.payload.user.phonenumber} найдено более одного контакта";
                        return response;
                    }
                    else if (entities.Count == 1)
                    {
                        contact = IntegrationServiceHelper.GetEntityByField(this.UserConnection, "Contact", "Id", entities.First().GetColumnValue("ContactId"));

                        if (contact.GetTypedColumnValue<string>("GivenName") == requestModel.payload.user.name)
                        {
                            UpdateContactTrcPersonalAccount(contact, requestModel);
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
                    string documentId = string.IsNullOrEmpty(requestModel.payload.documentid) ? requestModel.payload.documentId : requestModel.payload.documentid;

                    request = IntegrationServiceHelper.GetEntityByField(this.UserConnection, "TrcRequest", "TrcRequestIdLK", documentId);

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
                }
            }
            else
            {
                response.Code = 500;
                response.Result = false;
                response.ReasonPhrase = $"Тип запроса {requestModel.type} не определен";
            }

            return response;
        }

        protected void UpdateContactTrcPersonalAccount(Entity contact, RequestServiceRequestModel data)
        {
            var isChanged = false;

            if (!contact.GetTypedColumnValue<bool>("TrcPersonalAccount"))
            {
                isChanged = true;
                contact.SetColumnValue("TrcPersonalAccount", true);
            }

            if (contact.GetTypedColumnValue<string>("TrcPersonalAccount") != data.payload.user.userId)
            {
                isChanged = true;
                contact.SetColumnValue("TrcProfitbaseLKId", data.payload.user.userId);
            }

            if (isChanged)
            {
                contact.Save(false);
            }
        }

        protected override void InitRequiredFields(List<string> requiredFields)
        {
            requiredFields.Add("type");
            requiredFields.Add("payload");
        }

        protected override void CheckRequiredFields(RequestServiceRequestModel request, RequestServiceResponseModel response)
        {
            base.CheckRequiredFields(request, response);

            
            if (response.Result)
            {
                if (request.type == "documentCreated")
                {
                    if (string.IsNullOrEmpty(request.payload.documentId))
                    {
                        response.ReasonPhrase = "Не заполнено обязательное поле payload.documentId";
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
                        if (string.IsNullOrEmpty(request.payload.documentId) && string.IsNullOrEmpty(request.payload.documentid))
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
                else if (request.type == "documentChangeStatus")
                {
                    if (string.IsNullOrEmpty(request.payload.nextStatus))
                    {
                        response.ReasonPhrase = "Не заполнено обязательное поле payload.nextStatus";
                        response.Result = false;
                        response.Code = 500;
                        return;
                    }

                    if (string.IsNullOrEmpty(request.payload.previousStatus))
                    {
                        response.ReasonPhrase = "Не заполнено обязательное поле payload.previousStatus";
                        response.Result = false;
                        response.Code = 500;
                        return;
                    }

                    if (string.IsNullOrEmpty(request.payload.documentId) && string.IsNullOrEmpty(request.payload.documentid))
                    {
                        response.ReasonPhrase = "Не заполнено обязательное поле payload.documentId";
                        response.Result = false;
                        response.Code = 500;
                        return;
                    }
                }
            }
        }
    }
}
