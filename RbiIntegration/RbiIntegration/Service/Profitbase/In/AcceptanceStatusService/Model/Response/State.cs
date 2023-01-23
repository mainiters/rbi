using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.Profitbase.In.AcceptanceStatusService.Model.Response
{
    /// <summary>
    /// Статус
    /// </summary>
    [DataContract]
    public class State
    {
        /// <summary>
        /// Наименование статуса
        /// </summary>
        [DataMember]
        public string name { get; set; }

        /// <summary>
        /// Цвет статуса HEX
        /// </summary>
        [DataMember]
        public string color { get; set; }
    }
}