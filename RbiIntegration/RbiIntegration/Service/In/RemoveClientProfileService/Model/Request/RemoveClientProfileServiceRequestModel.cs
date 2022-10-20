using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.In.RemoveClientProfileService.Model.Request
{
    /// <summary>
    /// Модель запроса
    /// </summary>
    [DataContract]
    public class RemoveClientProfileServiceRequestModel : BaseModel
    {
        /// <summary>
        /// ИД контакта
        /// </summary>
        [DataMember]
        public string TrcContactId { get; set; }

        /// <summary>
        /// Дата и время удаления
        /// </summary>
        [DataMember]
        public string TrcDomopultDeletedOn { get; set; }
    }
}
