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
    /// Объект с информацией о необходимости доработки и предоставлении дополнительных данных со стороны клиента
    /// </summary>
    [DataContract]
    public class infoRevision
    {
        /// <summary>
        /// Комментарий клиента
        /// </summary>
        [DataMember]
        public string comment { get; set; }

        /// <summary>
        /// Документ от клиента при необходимости
        /// </summary>
        [DataMember]
        public doc[] doc { get; set; }
    }
}