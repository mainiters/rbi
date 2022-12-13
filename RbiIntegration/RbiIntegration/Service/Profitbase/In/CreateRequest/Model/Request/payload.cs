using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.Profitbase.In.CreateRequest.Model.Request
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
    }
}
