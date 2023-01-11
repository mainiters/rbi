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
    /// Подсказка клиенту
    /// </summary>
    [DataContract]
    public class Prompt
    {
        /// <summary>
        /// Расположение блока на странице
        /// </summary>
        [DataMember]
        public int position { get; set; }

        /// <summary>
        /// Текстовая подсказка клиенту
        /// </summary>
        [DataMember]
        public string text { get; set; }
    }
}