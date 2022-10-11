using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.In.CreateClientService.Model.Request
{
    /// <summary>
    /// Информация о email
    /// </summary>
    [DataContract]
    public class EmailData
    {
        /// <summary>
        /// Признак основного телефона
        /// </summary>
        [DataMember]
        public bool Basic { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [DataMember]
        public string Email { get; set; }
    }
}
