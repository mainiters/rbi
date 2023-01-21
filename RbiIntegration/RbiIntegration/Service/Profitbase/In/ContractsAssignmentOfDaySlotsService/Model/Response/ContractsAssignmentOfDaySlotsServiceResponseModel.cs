using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.Profitbase.In.ContractsAssignmentOfDaySlotsService.Model.Response
{
    /// <summary>
    /// Модель ответа
    /// </summary>
    [DataContract]
    public class ContractsAssignmentOfDaySlotsServiceResponseModel : BaseResponse
    {
        /// <summary>
        /// Даты
        /// </summary>
        [DataMember]
        public string[] date { get; set; }

    }
}
