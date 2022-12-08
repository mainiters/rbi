using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.In.Profitbase.CreateRequest.Model.Request
{
    /// <summary>
    /// Связанная заявка
    /// </summary>
    [DataContract]
    public class linkingDocument
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
    }
}