using RbiIntegration.Service.In.Profitbase.Enrichment;
using RbiIntegration.Service.In.Profitbase.EnrichmentService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Terrasoft.Core;
using Terrasoft.Core.DB;
using Terrasoft.Core.Entities;

namespace RbiIntegration.Service.BaseClasses
{
    /// <summary>
    /// Базовая обертка для вхаимодействия с сервисами
    /// </summary>
    public class ServiceWrapper
    {
        /// <summary>
        /// Соединение пользователя
        /// </summary>
        protected UserConnection _userConnection { get; set; }

        /// <summary>
        /// Код сервиса
        /// </summary>
        protected string _serviceCode { get; set; }

        /// <summary>
        /// Урл для принудительной отправки запроса на указанный адрес сервиса
        /// </summary>
        protected string _defaultUrl { get; set; }

        /// <summary>
        /// Конструктор враппера
        /// </summary>
        /// <param name="userConnection">Соединение пользователя</param>
        /// <param name="serviceCode">Код сервиса</param>
        public ServiceWrapper(UserConnection userConnection, string serviceCode, string defaultUrl = null)
        {
            this._userConnection = userConnection;
            this._serviceCode = serviceCode;
            this._defaultUrl = defaultUrl;
        }

        /// <summary>
        /// Метод отправки запроса в сервис
        /// </summary>
        /// <param name="id">Идентификатор сущности для формирования запроса</param>
        public BaseResponse SendRequest(params Guid[] id)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

            DateTime requestInitDate = DateTime.Now;

            var errorStr = string.Empty;
            var uid = string.Empty;

            var serviceParams = new IntegrationServiceParams();
            try
            {
                serviceParams = IntegrationServiceHelper.GetEntityNameByServiceCode(this._userConnection, this._serviceCode, this._defaultUrl);

                var serviceClient = GetServiceByServiceParams(serviceParams);

                return serviceClient.CallService(id);
            }
            catch (Exception ex)
            {
                errorStr = $"{ex.Message} /n{ex.StackTrace}";

                IntegrationServiceHelper.Log(this._userConnection, serviceParams, requestInitDate, null, uid, errorStr, string.Empty, string.Empty, 0);
            }

            return null;
        }

        /// <summary>
        /// Получение клиента сервиса по параметрам
        /// </summary>
        /// <param name="serviceParams"></param>
        /// <returns></returns>
        protected BaseOutService GetServiceByServiceParams(IntegrationServiceParams serviceParams)
        {
            switch (serviceParams.ServiceName)
            {
                case "Enrichment":
                    return new EnrichmentService(this._userConnection, serviceParams);
                default:
                    throw new Exception($"Неизвестный код сервиса {serviceParams.ServiceName}");
            }
        }
    }
}
