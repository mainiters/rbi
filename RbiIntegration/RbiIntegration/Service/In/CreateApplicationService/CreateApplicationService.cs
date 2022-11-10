using RbiIntegration.Service.BaseClasses;
using RbiIntegration.Service.In.CreateApplicationService.Model.Request;
using RbiIntegration.Service.In.CreateApplicationService.Model.Response;
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

namespace RbiIntegration.Service.In.CreateApplicationService
{
    /// <summary>
    /// Сервис создания заявки
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class CreateApplicationService : BaseRbiService<CreateApplicationServiceRequestModel, CreateApplicationServiceResponseModel>
    {
        protected override Guid GetIntegrationServiceId()
        {
            return CrmConstants.TrcIntegrationServices.CreateApplication;
        }

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        protected override CreateApplicationServiceResponseModel ProcessBusinessLogic(CreateApplicationServiceRequestModel requestModel, CreateApplicationServiceResponseModel response)
        {
            Entity service = null;
            Entity contact = null;
            Entity product = null;
            Entity status = null;

            try
            {
                service = IntegrationServiceHelper.GetEntityByField(this.UserConnection, "TrcService", "Id", requestModel.TrcServiceId);
            }
            catch (Exception ex)
            {
                response.Result = false;
                response.Code = 304042;
                response.ReasonPhrase = $"Услуга с id {requestModel.TrcServiceId} не найдена";
                return response;
            }

            try
            {
                contact = IntegrationServiceHelper.GetEntityByField(this.UserConnection, "Contact", "Id", requestModel.TrcContactId);
            }
            catch (Exception ex)
            {
                response.Result = false;
                response.Code = 104002;
                response.ReasonPhrase = $"Контакт с id {requestModel.TrcContactId} не найден";
                return response;
            }

            try
            {
                product = IntegrationServiceHelper.GetEntityByField(this.UserConnection, "Product", "Id", requestModel.ProductId);
            }
            catch (Exception ex)
            {
                response.Result = false;
                response.Code = 104004;
                response.ReasonPhrase = $"Объект с id {requestModel.ProductId} не найден";
                return response;
            }

            try
            {
                status = IntegrationServiceHelper.GetEntityByField(this.UserConnection, "TrcRequestStatus", "Id", requestModel.TrcRequestStatusId);
            }
            catch (Exception ex)
            {
                response.Result = false;
                response.Code = 304002;
                response.ReasonPhrase = $"Статус заявки с id {requestModel.TrcRequestStatusId} не найден";
                return response;
            }

            var request = IntegrationServiceHelper.InsertEntityWithFields(this.UserConnection, "TrcRequest", new Dictionary<string, object>()
            {
                { "TrcServiceId", service.PrimaryColumnValue },
                { "TrcContactId", contact.PrimaryColumnValue },
                { "TrcObjectProfitbaseId", product.PrimaryColumnValue },
                { "TrcRequestStatusId", status.PrimaryColumnValue },
                { "TrcDomopultApplicationId", requestModel.TrcApplicationDomopultId },
                { "TrcName", requestModel.TrcName },
                { "TrcDescription", requestModel.TrcDescription },
                { "TrcRequestTypeId", Guid.Parse("c43e49af-89b4-4754-ab9c-172b04faae1b") },
                { "TrcRequestSourceId", Guid.Parse("403e346b-080c-4c54-8c13-e66493d1607d") },
                { "TrcDomopultCreatedOn", DateTime.Parse(requestModel.TrcDomopultCreatedOn) }
            });

            response.TrcRequestId = request.PrimaryColumnValue.ToString();

            return response;
        }

        protected override void InitRequiredFields(List<string> requiredFields)
        {
            requiredFields.Add("TrcServiceId");
            requiredFields.Add("TrcContactId");
            requiredFields.Add("ProductId");
            requiredFields.Add("TrcApplicationDomopultId");
            requiredFields.Add("TrcRequestStatusId");
            requiredFields.Add("TrcName");
            requiredFields.Add("TrcDomopultCreatedOn");
        }
    }
}
