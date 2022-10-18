using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.In.AddClientObjectService.Model.Request
{
    /// <summary>
    /// Модель проверки существования клиента
    /// </summary>
    [DataContract]
    public class AddClientObjectServiceRequestModel : BaseModel
    {
        /// <summary>
        /// Идентификатор контакта
        /// </summary>
        [DataMember]
        public string TrcContactId { get; set; }

        /// <summary>
        /// Идентификатор помещения
        /// </summary>
        [DataMember]
        public string ProductId { get; set; }

        /// <summary>
        /// Идентификатор роли
        /// </summary>
        [DataMember]
        public string TrcContactRoleForObjectId { get; set; }

        /// <summary>
        /// Лицевой счет
        /// </summary>
        [DataMember]
        public string TrcPersonalAccount { get; set; }

        /// <summary>
        /// Телефоны
        /// </summary>
        [DataMember]
        public PhoneData[] Phones { get; set; }
    }
}
