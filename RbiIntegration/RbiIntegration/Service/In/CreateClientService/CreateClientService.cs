using RbiIntegration.Service.BaseClasses;
using RbiIntegration.Service.In.CreateClientService.Model.Request;
using RbiIntegration.Service.In.CreateClientService.Model.Response;
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

namespace RbiIntegration.Service.In.CreateClientService
{
    /// <summary>
    /// Сервис псоздания клиента
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class CreateClientService : BaseRbiService<CreateClientServiceRequestModel, CreateClientServiceResponseModel>
    {
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        protected override CreateClientServiceResponseModel ProcessBusinessLogic(CreateClientServiceRequestModel requestModel, CreateClientServiceResponseModel response)
        {
            var values = new Dictionary<string, object>()
            {
                { "Name", requestModel.Name },
                { "TrcDomopultID", requestModel.TrcDomopultID },
                { "TrcDomopultCreatedOn", DateTime.Parse(requestModel.TrcDomopultCreatedOn) }
            };

            var contact = IntegrationServiceHelper.InsertEntityWithFields(this.UserConnection, "Contact", values);

            response.TrcContactId = contact.PrimaryColumnValue.ToString();

            ProcessCommunications(requestModel, contact.PrimaryColumnValue);

            var communicationsDict = new Dictionary<string, object>();

            if (requestModel.Phones != null && requestModel.Phones.Length > 0 && requestModel.Phones.Count(e => e.Basic == true) > 0)
            {
                communicationsDict.Add("MobilePhone", IntegrationServiceHelper.MaskPhone(requestModel.Phones.First(e => e.Basic == true).Phone));
            }

            if (requestModel.Emails != null && requestModel.Emails.Length > 0 && requestModel.Emails.Count(e => e.Basic) > 0)
            {
                communicationsDict.Add("Email", requestModel.Emails.First(e => e.Basic).Email);
            }

            if (communicationsDict.Count > 0)
            {
                IntegrationServiceHelper.InsertOrUpdateEntity(this.UserConnection, "Contact", "Id", contact.PrimaryColumnValue, communicationsDict);
            }

            return response;
        }

        /// <summary>
        /// Обработка средств связи контакта
        /// </summary>
        /// <param name="requestModel">Моддель запроса</param>
        /// <param name="contactId">Идентификтаор контакта</param>
        protected void ProcessCommunications(CreateClientServiceRequestModel requestModel, Guid contactId)
        {
            if (requestModel != null)
            {
                if (requestModel.Phones != null && requestModel.Phones.Length > 0)
                {
                    foreach (var item in requestModel.Phones.Where(e => !e.Basic == true))
                    {
                        IntegrationServiceHelper.InsertEntityWithFields(this.UserConnection, "ContactCommunication", new Dictionary<string, object>()
                        {
                            { "ContactId", contactId },
                            { "Number", IntegrationServiceHelper.MaskPhone(item.Phone) },
                            { "CommunicationTypeId", "2b387201-67cc-df11-9b2a-001d60e938c6" }
                        });
                    }
                }

                if (requestModel.Emails != null && requestModel.Emails.Length > 0)
                {
                    foreach (var item in requestModel.Emails.Where(e => !e.Basic))
                    {
                        IntegrationServiceHelper.InsertEntityWithFields(this.UserConnection, "ContactCommunication", new Dictionary<string, object>()
                        {
                            { "ContactId", contactId },
                            { "Number", item.Email },
                            { "CommunicationTypeId", "ee1c85c3-cfcb-df11-9b2a-001d60e938c6" }
                        });
                    }
                }
            }
        }

        protected override void InitRequiredFields(List<string> requiredFields)
        {
            requiredFields.Add("Phones");
            requiredFields.Add("TrcDomopultID");
            requiredFields.Add("Name");
            requiredFields.Add("ProductId");
            requiredFields.Add("TrcContactRoleForObjectId");
            requiredFields.Add("TrcDomopultCreatedOn");
        }

        protected override void CheckRequiredFields(CreateClientServiceRequestModel request, CreateClientServiceResponseModel response)
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
