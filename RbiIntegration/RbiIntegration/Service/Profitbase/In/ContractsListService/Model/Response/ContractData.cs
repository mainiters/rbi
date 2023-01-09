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
    public class ContractData : BaseResponse
    {
        /// <summary>
        /// Идентификатор договора в CRM
        /// </summary>
        [DataMember]
        public string contractId { get; set; }

        /// <summary>
        /// Номер договора
        /// </summary>
        [DataMember]
        public string contractNum { get; set; }

        /// <summary>
        /// Дата договора
        /// </summary>
        [DataMember]
        public string contractDate { get; set; }

        /// <summary>
        /// Статус взаиморасчетов
        /// </summary>
        [DataMember]
        public KeyValueData stateMutualSett { get; set; }

        /// <summary>
        /// Статус договора
        /// </summary>
        [DataMember]
        public KeyValueData stateContract { get; set; }
    }
}
