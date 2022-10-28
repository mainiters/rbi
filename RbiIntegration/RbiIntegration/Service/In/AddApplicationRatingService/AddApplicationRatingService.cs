using RbiIntegration.Service.BaseClasses;
using RbiIntegration.Service.In.AddApplicationRatingService.Model.Request;
using RbiIntegration.Service.In.AddApplicationRatingService.Model.Response;
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

namespace RbiIntegration.Service.In.AddApplicationRatingService
{
    /// <summary>
    /// Сервис создания заявки
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class AddApplicationRatingService : BaseRbiService<AddApplicationRatingServiceRequestModel, AddApplicationRatingServiceResponseModel>
    {
        protected override Guid GetIntegrationServiceId()
        {
            return CrmConstants.TrcIntegrationServices.AddApplicationRating;
        }

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        protected override AddApplicationRatingServiceResponseModel ProcessBusinessLogic(AddApplicationRatingServiceRequestModel requestModel, AddApplicationRatingServiceResponseModel response)
        {
            Entity request = null;

            try
            {
                request = IntegrationServiceHelper.GetEntityByField(this.UserConnection, "TrcRequest", "Id", requestModel.TrcRequestId);
            }
            catch (Exception ex)
            {
                response.Result = false;
                response.Code = 304401;
                response.ReasonPhrase = $"Заявка с id {requestModel.TrcRequestId} не найдена";
                return response;
            }

            var rating = IntegrationServiceHelper.InsertEntityWithFields(this.UserConnection, "TrcRequest", new Dictionary<string, object>()
            {
                { "TrcRequestId", request.PrimaryColumnValue },
                { "TrcRating", requestModel.TrcRating },
                { "TrcRatingDescription", requestModel.TrcRatingDescription },
            });

            return response;
        }

        protected override void InitRequiredFields(List<string> requiredFields)
        {
            requiredFields.Add("TrcRequestId");
            requiredFields.Add("TrcRating");
            requiredFields.Add("TrcRatingDescription");
        }
    }
}
