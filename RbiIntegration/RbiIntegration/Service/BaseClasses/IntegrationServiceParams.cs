using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.BaseClasses
{
    /// <summary>
    /// Параметры для вызова интеграционного сервиса
    /// </summary>
    public class IntegrationServiceParams
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название сервиса
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Название базовой сущности
        /// </summary>
        public string EntitySchemaName { get; set; }

        /// <summary>
        /// URL сервиса
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Логин сервиса
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль сервиса
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Токен
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Тип запроса
        /// </summary>
        public string RequestType { get; set; }
    }
}
