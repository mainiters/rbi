using RbiIntegration.Service.BaseClasses;
using RbiIntegration.Service.Profitbase.Out.AuthToken.Model.Response;
using RbiIntegration.Service.Profitbase.Out.Enrichment.Model.Response;
using RbiIntegration.Service.Profitbase.Out.GetFile.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.DB;

namespace RbiIntegration.Service.Profitbase.Out.GetFile.Handler
{
    /// <summary>
    /// Обработчик ответа сервиса
    /// </summary>
    public class GetFileServiceResponseHandler : BaseResponseHandler
    {
        /// <summary>
        /// Констурктор обработчика ответа сервиса
        /// </summary>
        /// <param name="userConnection">Соединение пользователя</param>
        /// <param name="serviceParams">Параметры интеграционного сервиса</param>
        public GetFileServiceResponseHandler(UserConnection userConnection, IntegrationServiceParams serviceParams)
            : base(userConnection, serviceParams)
        {

        }

        public override void Handle(BaseResponse response, params string[] id)
        {
            var responseModel = response as GetFileServiceResponseModel;

            try
            {
                string requestId = string.Empty;

                if (id.Length > 1)
                {
                    requestId = id[1];
                }
                else
                {
                    var request = IntegrationServiceHelper.GetEntityByField(this._userConnection, "TrcRequest", "TrcRequestIdLK", responseModel.documentId);
                    requestId = request.PrimaryColumnValue.ToString();
                }

                byte[] bytesFile = Convert.FromBase64String(responseModel.base64Content);

                IntegrationServiceHelper.InsertOrUpdateEntity(this._userConnection, "TrcRequestFile", "Name", responseModel.name, new Dictionary<string, object>()
                {
                    { "TrcRequestId", requestId },
                    { "Name", responseModel.name },
                    { "TypeId", Guid.Parse("529BC2F8-0EE0-DF11-971B-001D60E938C6") },
                    { "SysFileStorageId", Guid.Parse("38AB9812-9BBA-4EB8-86D0-8F352CD0229C") },
                    { "Version", 1 },
                    { "Data", bytesFile }
                },
                new Dictionary<string, object>()
                {
                    { "TrcRequest", requestId }
                });
            }
            catch (Exception ex)
            {
            }

            responseModel.base64Content = "...";
        }
    }
}
