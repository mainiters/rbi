using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.Profitbase.In.ContractsAssignmentOfTimeSlotsService.Model.Request
{
    /// <summary>
    /// Модель запроса
    /// </summary>
    [DataContract]
    public class ContractsAssignmentOfTimeSlotsServiceRequestModel : BaseModel
    {
        /// <summary>
        /// ID договора
        /// </summary>
        [DataMember]
        public string contractId { get; set; }

        /// <summary>
        /// Дата, выбранная клиентом
        /// </summary>
        [DataMember]
        public string date { get; set; }
    }
}
