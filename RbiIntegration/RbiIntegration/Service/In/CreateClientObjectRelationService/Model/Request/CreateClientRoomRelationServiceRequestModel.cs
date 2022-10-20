using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.In.CreateClientObjectRelationService.Model.Request
{
    /// <summary>
    /// Модель запроса
    /// </summary>
    [DataContract]
    public class CreateClientObjectRelationServiceRequestModel : BaseModel
    {
        /// <summary>
        /// Id Контакта в CRM
        /// </summary>
        [DataMember]
        public string TrcContactId { get; set; }

        /// <summary>
        /// Id помещения в CRM
        /// </summary>
        [DataMember]
        public string TrcObjectId { get; set; }

        /// <summary>
        /// Id роли в CRM
        /// </summary>
        [DataMember]
        public string TrcContactRoleForObjectId { get; set; }

        /// <summary>
        /// Лицевой счет
        /// </summary>
        [DataMember]
        public string TrcPersonalAccount { get; set; }
    }
}
