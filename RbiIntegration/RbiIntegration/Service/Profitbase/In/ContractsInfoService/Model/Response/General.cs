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
    /// Общая информация по договору в виде таблицы
    /// </summary>
    [DataContract]
    public class General
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
        /// Массив значений о договоре
        /// </summary>
        [DataMember]
        public KeyValueData[] data { get; set; }

        /// <summary>
        /// Массив значений о покупателях
        /// </summary>
        [DataMember]
        public Buyer[] buyers { get; set; }
    }
}