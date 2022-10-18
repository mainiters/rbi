using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.In.AddClientObjectService.Model.Request
{
    /// <summary>
    /// Информация о телефоне
    /// </summary>
    [DataContract]
    public class PhoneData
    {
        /// <summary>
        /// Признак основного телефона
        /// </summary>
        [DataMember]
        public bool Basic { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        [DataMember]
        public string Phone { get; set; }
    }
}
