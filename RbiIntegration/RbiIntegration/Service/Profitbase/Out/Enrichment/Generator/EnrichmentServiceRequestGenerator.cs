using RbiIntegration.Service.BaseClasses;
using RbiIntegration.Service.Profitbase.Out.AuthToken.Model.Response;
using RbiIntegration.Service.Profitbase.Out.Enrichment.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;

namespace RbiIntegration.Service.Profitbase.Out.Enrichment.Generator
{
    /// <summary>
    /// Генератор запроса
    /// </summary>
    public class EnrichmentServiceRequestGenerator : BaseRequestGenerator
    {
        /// <summary>
        /// Констурктор генератора
        /// </summary>
        /// <param name="userConnection">Соединение пользователя</param>
        /// <param name="serviceParams">Параметры сервиса</param>
        public EnrichmentServiceRequestGenerator(UserConnection userConnection, IntegrationServiceParams serviceParams)
            : base(userConnection, serviceParams)
        {

        }

        /// <summary>
        /// Генерация модели запроса
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override BaseModel GenerateModel(params string[] id)
        {
            var data = ReadEntityData(id);

            return new EnrichmentServiceRequestModel()
            {
                id = data[id.First()].GetTypedColumnValue<string>("TrcRequestIdLK")
            };
        }
    }
}
