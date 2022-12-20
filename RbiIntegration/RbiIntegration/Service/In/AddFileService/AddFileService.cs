using RbiIntegration.Service.BaseClasses;
using RbiIntegration.Service.In.AddFileService.Model.Request;
using RbiIntegration.Service.In.AddFileService.Model.Response;
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

namespace RbiIntegration.Service.In.AddFileService
{
    /// <summary>
    /// Сервис получения файлов
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class AddFileService : BaseRbiService<AddFileServiceRequestModel, AddFileServiceResponseModel>
    {
        public AddFileService(UserConnection UserConnection) : base(UserConnection)
        {

        }

        protected override Guid GetIntegrationServiceId()
        {
            return CrmConstants.TrcIntegrationServices.AddFile;
        }

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        protected override AddFileServiceResponseModel ProcessBusinessLogic(AddFileServiceRequestModel requestModel, AddFileServiceResponseModel response)
        {
            Entity request = null;

            try
            {
                request = IntegrationServiceHelper.GetEntityByField(this.UserConnection, "TrcRequest", "Id", requestModel.TrcRequestId);
            }
            catch (Exception ex)
            {
                response.Result = false;
                response.Code = 304041;
                response.ReasonPhrase = $"Заявка с id {requestModel.TrcRequestId} не найдена";
                return response;
            }

            foreach (var item in requestModel.File)
            {
                IntegrationServiceHelper.InsertEntity(this.UserConnection, "TrcRequestFile", new Dictionary<string, object>()
                {
                    { "TrcRequestId", request.PrimaryColumnValue },
                    { "TypeId", Guid.Parse("539BC2F8-0EE0-DF11-971B-001D60E938C6") },
                    { "Name", item.Link },
                    { "Notes", item.FileName }
                });
            }

            return response;
        }

        protected override void InitRequiredFields(List<string> requiredFields)
        {
            requiredFields.Add("TrcRequestId");
            requiredFields.Add("File");
        }

        protected override void CheckRequiredFields(AddFileServiceRequestModel request, AddFileServiceResponseModel response)
        {
            base.CheckRequiredFields(request, response);

            if (response.Result)
            {
                foreach (var item in request.File)
                {
                    if (string.IsNullOrEmpty(item.FileName))
                    {
                        response.ReasonPhrase = $"Обязательное поле FileName не заполнено";
                        response.Code = 304001;
                        response.Result = false;
                        return;
                    }

                    if (string.IsNullOrEmpty(item.Link))
                    {
                        response.ReasonPhrase = $"Обязательное поле Link не заполнено";
                        response.Code = 304001;
                        response.Result = false;
                        return;
                    }
                }
            }
        }
    }
}
