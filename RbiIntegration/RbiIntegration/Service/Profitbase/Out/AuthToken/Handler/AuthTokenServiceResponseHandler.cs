using RbiIntegration.Service.BaseClasses;
using RbiIntegration.Service.Profitbase.Out.AuthToken.Model.Response;
using RbiIntegration.Service.Profitbase.Out.Enrichment.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.DB;

namespace RbiIntegration.Service.Profitbase.Out.AuthToken.Handler
{
    /// <summary>
    /// Обработчик ответа сервиса
    /// </summary>
    public class AuthTokenServiceResponseHandler : BaseResponseHandler
    {
        /// <summary>
        /// Констурктор обработчика ответа сервиса
        /// </summary>
        /// <param name="userConnection">Соединение пользователя</param>
        /// <param name="serviceParams">Параметры интеграционного сервиса</param>
        public AuthTokenServiceResponseHandler(UserConnection userConnection, IntegrationServiceParams serviceParams)
            : base(userConnection, serviceParams)
        {

        }

        public override void Handle(BaseResponse response, params string[] id)
        {
            var responseModel = response as AuthTokenServiceResponseModel;

            Update update = new Update(this._userConnection, "TrcIntegrationServices")
                .Set("TrcToken", Column.Parameter(responseModel.token))
                .Where("Id").IsEqual(Column.Parameter(this._serviceParams.Id)) as Update;

            //update.Execute();
        }
    }
}
