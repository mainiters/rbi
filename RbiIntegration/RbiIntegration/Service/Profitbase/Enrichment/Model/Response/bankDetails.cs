using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.In.Profitbase.Enrichment.Model.Response
{
    /// <summary>
    /// Объект с банковскими реквизитами
    /// </summary>
    [DataContract]
    public class bankDetails
    {
        /// <summary>
        /// Название банка
        /// </summary>
        [DataMember]
        public string bankname { get; set; }

        /// <summary>
        /// Сумма возврата
        /// </summary>
        [DataMember]
        public string sum { get; set; }

        /// <summary>
        /// ФИО получателя
        /// </summary>
        [DataMember]
        public string fio { get; set; }

        /// <summary>
        /// Номер расчётного счёта
        /// </summary>
        [DataMember]
        public string accountnumber { get; set; }

        /// <summary>
        /// ИНН банка
        /// </summary>
        [DataMember]
        public string inn { get; set; }

        /// <summary>
        /// КПП банка
        /// </summary>
        [DataMember]
        public string kpp { get; set; }

        /// <summary>
        /// БИК
        /// </summary>
        [DataMember]
        public string bic { get; set; }

        /// <summary>
        /// Корр. счёт
        /// </summary>
        [DataMember]
        public string correspondentAccount { get; set; }
    }
}