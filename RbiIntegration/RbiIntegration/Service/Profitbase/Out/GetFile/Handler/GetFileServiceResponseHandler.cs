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
        }
    }
}
