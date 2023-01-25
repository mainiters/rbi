using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.Profitbase.In.ContractsPaymentScheduleService.Model.Request
{
    /// <summary>
    /// Модель запроса
    /// </summary>
    [DataContract]
    public class ContractsPaymentScheduleServiceRequestModel : BaseModel
    {
        /// <summary>
        /// Id договора
        /// </summary>
        [DataMember]
        public string contractId { get; set; }

    }
}
