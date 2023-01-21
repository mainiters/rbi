using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.Profitbase.In.ContractsAssignmentOfTimeSlotsService.Model.Response
{
    /// <summary>
    /// Модель ответа
    /// </summary>
    [DataContract]
    public class ContractsAssignmentOfTimeSlotsServiceResponseModel : BaseResponse
    {
        /// <summary>
        /// Доступное время начала осмотра
        /// </summary>
        [DataMember]
        public string[] time { get; set; }

    }
}
