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
    public class CreateClientService : BaseService
    {
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public BaseResponse CreateClient(CreateClientServiceRequestModel requestModel)
        {
            DateTime requestInitDate = DateTime.Now;

            var res = new CreateClientServiceResponseModel();
            var request = IntegrationServiceHelper.ToJson(requestModel);

            var uid = string.Empty;
            var title = string.Empty;

            try
            {
                var values = new Dictionary<string, object>()
                {
                    { "Name", requestModel.Name },
                    { "TrcDomopultID", requestModel.TrcDomopultID },
                    { "TrcDomopultCreatedOn", DateTime.Parse(requestModel.TrcDomopultCreatedOn) }
                };

                if (requestModel.Phones != null && requestModel.Phones.Length > 0 && requestModel.Phones.Count(e => e.Basic) > 0)
                {
                    values.Add("MobilePhone", IntegrationServiceHelper.MaskPhone(requestModel.Phones.First(e => e.Basic).Phone));
                }

                if (requestModel.Emails != null && requestModel.Emails.Length > 0 && requestModel.Emails.Count(e => e.Basic) > 0)
                {
                    values.Add("Email", requestModel.Emails.First(e => e.Basic).Email);
                }

                var contact = IntegrationServiceHelper.InsertEntityWithFields(this.UserConnection, "Contact", values);

                res.TrcContactId = contact.PrimaryColumnValue.ToString();

                ProcessCommunications(requestModel, contact.PrimaryColumnValue);
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
                IntegrationServiceHelper.Log(UserConnection, new IntegrationServiceParams() { Id = TrcIntegrationServices.CreateClient }, requestInitDate, title, uid, res.Exception, request, IntegrationServiceHelper.ToJson(res));
            }

            return res;
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
                    foreach (var item in requestModel.Phones.Where(e => !e.Basic))
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
    }
}
