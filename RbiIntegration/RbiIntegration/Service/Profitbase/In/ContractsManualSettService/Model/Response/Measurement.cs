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
    /// Таблица о произведенных переобмерах
    /// </summary>
    [DataContract]
    public class Measurement
    {
        /// <summary>
        /// Расположение блока на странице
        /// </summary>
        [DataMember]
        public int position { get; set; }

        /// <summary>
        /// Заголовок таблицы
        /// </summary>
        [DataMember]
        public string headInfo { get; set; }

        /// <summary>
        /// Массив значений о переобмерах
        /// </summary>
        [DataMember]
        public KeyValueData[] data { get; set; }
    }
}
