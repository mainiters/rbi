using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.Profitbase.In.ContractsManualSettService.Model.Response
{
    /// <summary>
    /// Модель ответа
    /// </summary>
    [DataContract]
    public class ContractsManualSettServiceResponseModel : BaseResponse
    {
        /// <summary>
        /// Идентификатор объекта недвижимости по договору
        /// </summary>
        [DataMember]
        public int objectId { get; set; }

        /// <summary>
        /// Таблица о произведенных переобмерах
        /// </summary>
        [DataMember]
        public Measurement measurements { get; set; }

        /// <summary>
        /// Уведомление клиенту
        /// </summary>
        [DataMember]
        public Alert alert { get; set; }

        /// <summary>
        /// Подсказка клиенту
        /// </summary>
        [DataMember]
        public Prompt prompt { get; set; }

        /// <summary>
        /// Документ для согласования клиентом
        /// </summary>
        [DataMember]
        public AgreemDoc agreemDoc { get; set; }

        /// <summary>
        /// Документ (результат, инструкция и т.д.)
        /// </summary>
        [DataMember]
        public Documents documents { get; set; }
    }
}
