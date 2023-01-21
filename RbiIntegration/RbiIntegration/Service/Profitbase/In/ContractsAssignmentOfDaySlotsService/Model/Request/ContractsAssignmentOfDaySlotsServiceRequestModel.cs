using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.Profitbase.In.ContractsAssignmentOfDaySlotsService.Model.Request
{
    /// <summary>
    /// Модель запроса
    /// </summary>
    [DataContract]
    public class ContractsAssignmentOfDaySlotsServiceRequestModel : BaseModel
    {
        /// <summary>
        /// ID договора
        /// </summary>
        [DataMember]
        public string contractId { get; set; }
    }
}
