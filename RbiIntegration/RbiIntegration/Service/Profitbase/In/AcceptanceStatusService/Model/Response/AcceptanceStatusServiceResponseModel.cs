using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.Profitbase.In.AcceptanceStatusService.Model.Response
{
    /// <summary>
    /// Модель ответа
    /// </summary>
    [DataContract]
    public class AcceptanceStatusServiceResponseModel : BaseResponse
    {
        /// <summary>
        /// Идентификатор объекта недвижимости по договору
        /// </summary>
        [DataMember]
        public int? objectId { get; set; }

        /// <summary>
        /// Информация о доступной приемке
        /// </summary>
        [DataMember]
        public Acceptance acceptance { get; set; }

        /// <summary>
        /// Список замечаний
        /// </summary>
        [DataMember]
        public Remarks remarks { get; set; }

        /// <summary>
        /// Уведомление клиенту
        /// </summary>
        [DataMember]
        public Alert alert { get; set; }

        /// <summary>
        /// Подсказка клиенту
        /// </summary>
        [DataMember]
        public Prompt prompt { get; set; }

        /// <summary>
        /// Документы
        /// </summary>
        [DataMember]
        public Documents documents { get; set; }
    }
}
