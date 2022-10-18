using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;

namespace RbiIntegration.Service.BaseClasses
{
    /// <summary>
    /// Базовый обработчик ответа от интеграционного сервиса
    /// </summary>
    public class BaseResponseHandler
    {
        /// <summary>
        /// Подключение пользователя
        /// </summary>
        protected UserConnection _userConnection;

        /// <summary>
        /// Параметры сервиса
        /// </summary>
        protected IntegrationServiceParams _serviceParams;

        /// <summary>
        /// Констурктор базового обработчика ответа от сервиса
        /// </summary>
        /// <param name="userConnection">Соединение пользователя</param>
        /// <param name="serviceParams">Параметры вызова сервиса</param>
        public BaseResponseHandler(UserConnection userConnection, IntegrationServiceParams serviceParams)
        {
            this._userConnection = userConnection;
            this._serviceParams = serviceParams;
        }

        /// <summary>
        /// Обработка ответа от сервиса
        /// </summary>
        /// <param name="response">Ответ сервиса</param>
        /// <param name="id">Идентификаторы записей базовой сущности</param>
        public virtual void Handle(BaseResponse response, params Guid[] id)
        {

        }
    }
}
