using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.In.CreateClientService.Model.Request
{
    /// <summary>
    /// Модель запроса
    /// </summary>
    [DataContract]
    public class CreateClientServiceRequestModel : BaseModel
    {
        /// <summary>
        /// Id Клиента в Домопульт
        /// </summary>
        [DataMember]
        public int TrcDomopultID { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Телефоны
        /// </summary>
        [DataMember]
        public PhoneData[] Phones { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [DataMember]
        public EmailData[] Emails { get; set; }

        /// <summary>
        /// Дата создания в Демопульт
        /// </summary>
        [DataMember]
        public string TrcDomopultCreatedOn { get; set; }

        /// <summary>
        /// ИД помещения
        /// </summary>
        [DataMember]
        public string ProductId { get; set; }

        /// <summary>
        /// ИД связи контакта с помещением
        /// </summary>
        [DataMember]
        public string TrcContactRoleForObjectId { get; set; }

    }
}
