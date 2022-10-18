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
    public class RemoveClientObjectRelationService : BaseService
    {
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public BaseResponse RemoveClientObjectRelation(RemoveClientObjectRelationServiceRequestModel requestModel)
        {
            DateTime requestInitDate = DateTime.Now;

            var res = new RemoveClientObjectRelationServiceResponseModel();
            var request = IntegrationServiceHelper.ToJson(requestModel);

            var uid = string.Empty;
            var title = string.Empty;

            try
            {
                var delete = new Delete(this.UserConnection)
                            .From("TrcConnectionObjectWithContact")
                            .Where("Id").IsEqual(Column.Parameter(requestModel.TrcConnectionObjectWithContactId));

                var removedCount = delete.Execute();

                res.Result = removedCount > 0;

                if (!res.Result)
                {
                    res.Code = 104006;
                    res.ReasonPhrase = $"Связь с id {requestModel.TrcConnectionObjectWithContactId} не найдена";
                }
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
                IntegrationServiceHelper.Log(UserConnection, new IntegrationServiceParams() { Id = TrcIntegrationServices.CreateClientObjectRelation }, requestInitDate, title, uid, res.Exception, request, IntegrationServiceHelper.ToJson(res));
            }

            return res;
        }

    }
}
