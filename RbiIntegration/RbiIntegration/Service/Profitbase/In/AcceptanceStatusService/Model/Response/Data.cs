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
    /// Замечания
    /// </summary>
    [DataContract]
    public class Data
    {
        /// <summary>
        /// Наименование замечания
        /// </summary>
        [DataMember]
        public string name { get; set; }

        /// <summary>
        /// Плановая дата устранения
        /// </summary>
        [DataMember]
        public string date { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        [DataMember]
        public State state { get; set; }

    }
}