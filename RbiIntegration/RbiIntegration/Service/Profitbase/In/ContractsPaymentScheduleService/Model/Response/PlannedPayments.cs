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
    /// Предстоящий платеж
    /// </summary>
    [DataContract]
    public class PlannedPayments
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
        public float amountPayment { get; set; }

        /// <summary>
        /// Признак просрочки
        /// </summary>
        [DataMember]
        public int latePayment { get; set; }
    }
}