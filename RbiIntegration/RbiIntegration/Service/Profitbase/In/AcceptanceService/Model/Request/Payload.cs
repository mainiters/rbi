using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.Profitbase.In.AcceptanceService.Model.Request
{
    [DataContract]
    public class Payload
    {
        /// <summary>
        /// Идентификатор договора
        /// </summary>
        [DataMember]
        public string contractId { get; set; }

        /// <summary>
        /// Решение о записи
        /// </summary>
        [DataMember]
        public int acceptance { get; set; }

        /// <summary>
        /// назначенная дата
        /// </summary>
        [DataMember]
        public string date { get; set; }

        /// <summary>
        /// назначенное время
        /// </summary>
        [DataMember]
        public string time { get; set; }

        /// <summary>
        /// Комментарий клиента
        /// </summary>
        [DataMember]
        public string comment { get; set; }

        /// <summary>
        /// Идентификаторы документа от клиента
        /// </summary>
        [DataMember]
        public string[] fileId { get; set; }
    }
}
