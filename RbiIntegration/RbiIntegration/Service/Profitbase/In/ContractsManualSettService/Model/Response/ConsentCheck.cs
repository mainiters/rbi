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
    /// Чек бокс согласия/не согласия
    /// </summary>
    [DataContract]
    public class ConsentCheck
    {
        /// <summary>
        /// Текст чек бокса
        /// </summary>
        [DataMember]
        public string name { get; set; }

        /// <summary>
        /// base64
        /// </summary>
        [DataMember]
        public int content { get; set; }
    }
}