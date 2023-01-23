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
    /// Список замечаний
    /// </summary>
    [DataContract]
    public class Remarks
    {
        /// <summary>
        /// Расположение блока на странице
        /// </summary>
        [DataMember]
        public int position { get; set; }

        /// <summary>
        /// Заголовок таблицы замечаний
        /// </summary>
        [DataMember]
        public string headRem { get; set; }

        /// <summary>
        /// Список замечаний
        /// </summary>
        [DataMember]
        public Data data { get; set; }

    }
}