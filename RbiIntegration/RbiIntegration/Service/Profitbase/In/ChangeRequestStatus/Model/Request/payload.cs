using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.Profitbase.In.ChangeRequestStatus.Model.Request
{
    /// <summary>
    /// Инфо по заявке
    /// </summary>
    [DataContract]
    public class payload
    {
        /// <summary>
        /// id заявки в ЛК
        /// </summary>
        [DataMember]
        public string documentId { get; set; }

        /// <summary>
        /// Тип заявки
        /// </summary>
        [DataMember]
        public string workflowType { get; set; }

        /// <summary>
        /// Статус, из которого заявка перешла
        /// </summary>
        [DataMember]
        public string previousStatus { get; set; }

        /// <summary>
        /// Статус, в который перешла заявка
        /// </summary>
        [DataMember]
        public string nextStatus { get; set; }
    }
}
