using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.Profitbase.In.ContractsInfoService.Model.Response
{
    /// <summary>
    /// Модель ответа
    /// </summary>
    [DataContract]
    public class ContractsInfoServiceResponseModel : BaseResponse
    {
        /// <summary>
        /// Идентификатор объекта недвижимости по договору
        /// </summary>
        [DataMember]
        public int? objectId { get; set; }

        /// <summary>
        /// Наличие таба взаиморасчеты
        /// </summary>
        [DataMember]
        public int availMutSett { get; set; }

        /// <summary>
        /// Наличие таба приемка
        /// </summary>
        [DataMember]
        public int availAcceptEstate { get; set; }

        /// <summary>
        /// Наличие таба График платежей
        /// </summary>
        [DataMember]
        public int availPaymentSchedule { get; set; }

        /// <summary>
        /// Общая информация по договору в виде таблицы
        /// </summary>
        [DataMember]
        public General general { get; set; }
    }
}
