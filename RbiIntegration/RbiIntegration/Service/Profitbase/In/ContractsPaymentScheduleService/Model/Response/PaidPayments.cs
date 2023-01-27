using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.Profitbase.In.ContractsPaymentScheduleService.Model.Response
{
    /// <summary>
    /// Совершенный платеж
    /// </summary>
    [DataContract]
    public class PaidPayments
    {
        /// <summary>
        /// Дата платежа
        /// </summary>
        [DataMember]
        public string datePayment { get; set; }

        /// <summary>
        /// Сумма платежа в рублях
        /// </summary>
        [DataMember]
        public decimal amountPayment { get; set; }
    }
}