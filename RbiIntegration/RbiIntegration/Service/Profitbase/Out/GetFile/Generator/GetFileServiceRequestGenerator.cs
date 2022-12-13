using RbiIntegration.Service.BaseClasses;
using RbiIntegration.Service.Profitbase.Out.GetFile.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;

namespace RbiIntegration.Service.Profitbase.Out.GetFile.Generator
{
    /// <summary>
    /// Генератор запроса
    /// </summary>
    public class GetFileServiceRequestGenerator : BaseRequestGenerator
    {
        /// <summary>
        /// Констурктор генератора
        /// </summary>
        /// <param name="userConnection">Соединение пользователя</param>
        /// <param name="serviceParams">Параметры сервиса</param>
        public GetFileServiceRequestGenerator(UserConnection userConnection, IntegrationServiceParams serviceParams)
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
            return new GetFileServiceRequestModel()
            {
                fileId = _serviceParams.Token
            };
        }
    }
}
