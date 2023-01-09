using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.Profitbase.In.ContractsListService.Model.Response
{
    /// <summary>
    /// Ключ-Значение
    /// </summary>
    [DataContract]
    public class KeyValueData : BaseResponse
    {
        /// <summary>
        /// Ключ
        /// </summary>
        [DataMember]
        public string name { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        [DataMember]
        public string value { get; set; }
    }
}
