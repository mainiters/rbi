using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.Profitbase.In.ContractsManualSettService.Model.Response
{
    /// <summary>
    /// Документ
    /// </summary>
    [DataContract]
    public class Document
    {
        /// <summary>
        /// Название документа
        /// </summary>
        [DataMember]
        public string name { get; set; }

        /// <summary>
        /// base64
        /// </summary>
        [DataMember]
        public string content { get; set; }

        /// <summary>
        /// тип файла
        /// </summary>
        [DataMember]
        public string type { get; set; }
    }
}