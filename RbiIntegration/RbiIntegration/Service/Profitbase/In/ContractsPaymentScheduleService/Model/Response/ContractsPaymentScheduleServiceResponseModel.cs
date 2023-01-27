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
    /// Модель ответа
    /// </summary>
    [DataContract]
    public class ContractsPaymentScheduleServiceResponseModel : BaseResponse
    {
        /// <summary>
        /// Идентификатор объекта недвижимости по договору
        /// </summary>
        [DataMember]
        public int objectId { get; set; }

        /// <summary>
        /// Номер договора
        /// </summary>
        [DataMember]
        public string contractNum { get; set; }

        /// <summary>
        /// Срок договора (действует до)
        /// </summary>
        [DataMember]
        public string dateUntil { get; set; }

        /// <summary>
        /// Сумма всего по договору в рублях
        /// </summary>
        [DataMember]
        public float amountContract { get; set; }

        /// <summary>
        /// Остаток по договору на сегодняшний день в рублях
        /// </summary>
        [DataMember]
        public float balanceContract { get; set; }

        /// <summary>
        /// Сумма ближайшего платежа по графику в рублях
        /// </summary>
        [DataMember]
        public decimal nextPayment { get; set; }

        /// <summary>
        /// Дата ближайшего платежа по графику
        /// </summary>
        [DataMember]
        public string nextPaymentDate { get; set; }

        /// <summary>
        /// Список предстоящих платежей (график)
        /// </summary>
        [DataMember]
        public List<PlannedPayments> plannedPayments { get; set; }

        /// <summary>
        /// Список совершённых платежей (график)
        /// </summary>
        [DataMember]
        public List<PaidPayments> paidPayments { get; set; }
    }
}
