using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.In.Profitbase.Enrichment.Model.Response
{
    /// <summary>
    /// Файл с документом
    /// </summary>
    [DataContract]
    public class doc
    {
        /// <summary>
        /// Идентификатор файла
        /// </summary>
        [DataMember]
        public string FileSrc { get; set; }

        /// <summary>
        /// Формат
        /// </summary>
        [DataMember]
        public string Extension { get; set; }

        /// <summary>
        /// Имя файла
        /// </summary>
        [DataMember]
        public string originName { get; set; }
    }
}