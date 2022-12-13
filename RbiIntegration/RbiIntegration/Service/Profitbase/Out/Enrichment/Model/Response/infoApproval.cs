using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.Profitbase.Out.Enrichment.Model.Response
{
    /// <summary>
    /// Объект с информацией о согласовании предложенного документа
    /// </summary>
    [DataContract]
    public class infoApproval
    {
        /// <summary>
        /// solution
        /// </summary>
        [DataMember]
        public bool? solution { get; set; }

        /// <summary>
        /// Комментарий клиента о причине отказа в согласовани
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