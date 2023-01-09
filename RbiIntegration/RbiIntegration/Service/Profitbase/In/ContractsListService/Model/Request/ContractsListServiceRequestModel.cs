using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.Profitbase.In.ContractsListService.Model.Request
{
    /// <summary>
    /// Модель запроса
    /// </summary>
    [DataContract]
    public class ContractsListServiceRequestModel : BaseModel
    {
        /// <summary>
        /// ID клиента
        /// </summary>
        [DataMember]
        public string clientId { get; set; }

        /// <summary>
        /// Телефон клиента
        /// </summary>
        [DataMember]
        public string phone { get; set; }
    }
}
