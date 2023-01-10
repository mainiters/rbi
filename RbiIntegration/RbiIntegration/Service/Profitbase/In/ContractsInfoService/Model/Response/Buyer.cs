using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.Profitbase.In.ContractsInfoService.Model.Response
{
    /// <summary>
    /// Ключ-Значение
    /// </summary>
    [DataContract]
    public class Buyer : BaseResponse
    {
        /// <summary>
        /// ФИО
        /// </summary>
        [DataMember]
        public string name { get; set; }

        /// <summary>
        /// Роль
        /// </summary>
        [DataMember]
        public string role { get; set; }

        /// <summary>
        /// Доля
        /// </summary>
        [DataMember]
        public string share { get; set; }
    }
}
