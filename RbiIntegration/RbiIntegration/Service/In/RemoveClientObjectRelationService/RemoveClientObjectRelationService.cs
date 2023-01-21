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
using Terrasoft.Core;
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
        public RemoveClientObjectRelationService(UserConnection UserConnection) : base(UserConnection)
        {

        }

        protected override Guid GetIntegrationServiceId(RemoveClientObjectRelationServiceRequestModel requestModel)
        {
            return CrmConstants.TrcIntegrationServices.RemoveClientObjectRelation;
        }

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        protected override RemoveClientObjectRelationServiceResponseModel ProcessBusinessLogic(RemoveClientObjectRelationServiceRequestModel requestModel, RemoveClientObjectRelationServiceResponseModel response)
        {
            try
            {
                var entity = IntegrationServiceHelper.GetEntityByField(this.UserConnection, "TrcConnectionObjectWithContact", "Id", requestModel.TrcConnectionObjectWithContactId);
                //response.Result = entity.Delete();

                entity.SetColumnValue("TrcDeletedOn", DateTime.Now);
                entity.SetColumnValue("TrcContactRoleForObjectId", Guid.Parse("2891f0a4-e6d5-4249-9f2f-a0c7da786826"));

                entity.Save(false);
            }
            catch (Exception ex)
            {
                response.Result = false;
                response.Code = 104006;
                response.ReasonPhrase = $"Связь с id {requestModel.TrcConnectionObjectWithContactId} не найдена";
            }

            return response;
        }

        protected override void InitRequiredFields(List<string> requiredFields)
        {
            requiredFields.Add("TrcConnectionObjectWithContactId");
        }
    }
}
