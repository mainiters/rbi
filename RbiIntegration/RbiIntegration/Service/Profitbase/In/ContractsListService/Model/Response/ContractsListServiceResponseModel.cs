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
    /// Модель ответа
    /// </summary>
    [DataContract]
    public class ContractsListServiceResponseModel : BaseResponse
    {
        /// <summary>
        /// Данные по договорам
        /// </summary>
        [DataMember]
        public ContractData[] ContractData { get; set; }
    }
}
