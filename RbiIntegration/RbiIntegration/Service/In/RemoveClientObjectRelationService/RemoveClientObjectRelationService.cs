using RbiIntegration.Service.BaseClasses;
using RbiIntegration.Service.In.RemoveClientObjectRelationService.Model.Request;
using RbiIntegration.Service.In.RemoveClientObjectRelationService.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core.DB;
using Terrasoft.Core.Entities;
using Terrasoft.Web.Common;
using static RbiIntegration.Service.BaseClasses.CrmConstants;

namespace RbiIntegration.Service.In.RemoveClientObjectRelationService
{
    /// <summary>
    /// Сервис удаления связи клиента с помещением
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class RemoveClientObjectRelationService : BaseRbiService<RemoveClientObjectRelationServiceRequestModel, RemoveClientObjectRelationServiceResponseModel>
    {
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        protected override RemoveClientObjectRelationServiceResponseModel ProcessBusinessLogic(RemoveClientObjectRelationServiceRequestModel requestModel, RemoveClientObjectRelationServiceResponseModel response)
        {
            var delete = new Delete(this.UserConnection)
                            .From("TrcConnectionObjectWithContact")
                            .Where("Id").IsEqual(Column.Parameter(requestModel.TrcConnectionObjectWithContactId));

            var removedCount = delete.Execute();

            response.Result = removedCount > 0;

            if (!response.Result)
            {
                response.Code = 104006;
                response.ReasonPhrase = $"Связь с id {requestModel.TrcConnectionObjectWithContactId} не найдена";
            }

            return response;
        }

        protected override void InitRequiredFields(List<string> requiredFields)
        {
        }
    }
}
