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
    /// Информация со свободной формой
    /// </summary>
    [DataContract]
    public class freeForm
    {
        /// <summary>
        /// Комментарий в свободной форме
        /// </summary>
        [DataMember]
        public string comment { get; set; }

        /// <summary>
        /// Файлы с документами
        /// </summary>
        [DataMember]
        public doc[] doc { get; set; }
    }
}