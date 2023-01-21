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
using Terrasoft.Core;
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
    public class CreateClientObjectRelationService : BaseRbiService<CreateClientObjectRelationServiceRequestModel, CreateClientObjectRelationServiceResponseModel>
    {
        public CreateClientObjectRelationService(UserConnection UserConnection) : base(UserConnection)
        {

        }

        protected override Guid GetIntegrationServiceId(CreateClientObjectRelationServiceRequestModel requestModel)
        {
            return CrmConstants.TrcIntegrationServices.CreateClientObjectRelation;
        }

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        protected override CreateClientObjectRelationServiceResponseModel ProcessBusinessLogic(CreateClientObjectRelationServiceRequestModel requestModel, CreateClientObjectRelationServiceResponseModel response)
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
                response.Result = false;
                response.Code = 104002;
                response.ReasonPhrase = $"Контакт с id {requestModel.TrcContactId} не найден";
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
                    product = IntegrationServiceHelper.FindLookupItem(this.UserConnection, "Product", requestModel.TrcObjectId, "Code", false, false).Entity;

                    product.SetColumnValue("TrcPersonalAccount", requestModel.TrcPersonalAccount);
                    product.Save();
                }
                catch (Exception ex)
                {
                    response.Result = false;
                    response.Code = 104004;
                    response.ReasonPhrase = $"Объект с кодом {requestModel.TrcObjectId} не найден";
                }
            }

            if (product != null)
            {
                var connectionObjectWithContact = IntegrationServiceHelper.InsertEntityWithFields(this.UserConnection, "TrcConnectionObjectWithContact", new Dictionary<string, object>()
                    {
                        { "TrcObjectId", product.PrimaryColumnValue },
                        { "TrcContactId", requestModel.TrcContactId },
                        { "TrcContactRoleForObjectId", requestModel.TrcContactRoleForObjectId },
                        { "TrcCreatedByDomopult", true }
                    });

                response.TrcConnectionObjectWithContactId = connectionObjectWithContact.PrimaryColumnValue.ToString();
            }

            return response;
        }

        protected override void InitRequiredFields(List<string> requiredFields)
        {
            requiredFields.Add("TrcContactId");
            requiredFields.Add("TrcObjectId");
            requiredFields.Add("TrcContactRoleForObjectId");
            requiredFields.Add("TrcPersonalAccount");
        }
    }
}
