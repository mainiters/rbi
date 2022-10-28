using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.In.CreateApplicationService.Model.Request
{
    /// <summary>
    /// Модель запроса
    /// </summary>
    [DataContract]
    public class CreateApplicationServiceRequestModel : BaseModel
    {
        /// <summary>
        /// Id Услуги в СРМ
        /// </summary>
        [DataMember]
        public string TrcServiceId { get; set; }

        /// <summary>
        /// Id Контакта в CRM
        /// </summary>
        [DataMember]
        public string TrcContactId { get; set; }

        /// <summary>
        /// Id Помещения в CRM
        /// </summary>
        [DataMember]
        public string ProductId { get; set; }

        /// <summary>
        /// Id Заявки в Домопульт
        /// </summary>
        [DataMember]
        public string TrcApplicationDomopultId { get; set; }

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

        /// <summary>
        /// Дата срздания в домопульт
        /// </summary>
        [DataMember]
        public string TrcDomopultCreatedOn { get; set; }
    }
}
