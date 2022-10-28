using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.In.UpdateApplicationService.Model.Request
{
    /// <summary>
    /// Модель запроса
    /// </summary>
    [DataContract]
    public class UpdateApplicationServiceRequestModel : BaseModel
    {
        /// <summary>
        /// Id Услуги в СРМ
        /// </summary>
        [DataMember]
        public string TrcServiceId { get; set; }

        /// <summary>
        /// Id обращения
        /// </summary>
        [DataMember]
        public string TrcRequestId { get; set; }

        /// <summary>
        /// Статус заявки
        /// </summary>
        [DataMember]
        public string TrcRequestStatusId { get; set; }

        /// <summary>
        /// Тема заявки
        /// </summary>
        [DataMember]
        public string TrcName { get; set; }

        /// <summary>
        /// Содержание заявки
        /// </summary>
        [DataMember]
        public string TrcDescription { get; set; }
    }
}
