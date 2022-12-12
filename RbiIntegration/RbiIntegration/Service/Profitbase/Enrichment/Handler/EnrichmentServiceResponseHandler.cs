using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;

namespace RbiIntegration.Service.Profitbase.Enrichment.Handler
{
    /// <summary>
    /// Обработчик ответа сервиса
    /// </summary>
    public class EnrichmentServiceResponseHandler : BaseResponseHandler
    {
        /// <summary>
        /// Констурктор обработчика ответа сервиса
        /// </summary>
        /// <param name="userConnection">Соединение пользователя</param>
        /// <param name="serviceParams">Параметры интеграционного сервиса</param>
        public EnrichmentServiceResponseHandler(UserConnection userConnection, IntegrationServiceParams serviceParams)
            : base(userConnection, serviceParams)
        {

        }

        public override void Handle(BaseResponse response, params Guid[] id)
        {
            
        }
    }
}
