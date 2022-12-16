using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.Profitbase.In.RequestService.Model.Request
{
    /// <summary>
    /// Инфо по заявке
    /// </summary>
    [DataContract]
    public class payload
    {
        /// <summary>
        /// Id созданной заявки
        /// </summary>
        [DataMember]
        public string documentId { get; set; }

        /// <summary>
        /// Id созданной заявки
        /// </summary>
        [DataMember]
        public string documentid { get; set; }

        /// <summary>
        /// Тип заявки
        /// </summary>
        [DataMember]
        public string workflowType { get; set; }

        /// <summary>
        /// Связанная заявка
        /// </summary>
        [DataMember]
        public linkingDocument linkingDocument { get; set; }

        /// <summary>
        /// Информация о пользователе
        /// </summary>
        [DataMember]
        public user user { get; set; }

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
