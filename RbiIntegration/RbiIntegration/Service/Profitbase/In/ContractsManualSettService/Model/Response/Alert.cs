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
    /// Уведомление клиенту
    /// </summary>
    [DataContract]
    public class Alert
    {
        /// <summary>
        /// Расположение блока на странице
        /// </summary>
        [DataMember]
        public int position { get; set; }

        /// <summary>
        /// Заголовок аллерта
        /// </summary>
        [DataMember]
        public string headAl { get; set; }

        /// <summary>
        /// Текст аллерта
        /// </summary>
        [DataMember]
        public string textAl { get; set; }

        /// <summary>
        /// Наличие кнопки для возврата
        /// </summary>
        [DataMember]
        public int buttonSurcharge { get; set; }

        /// <summary>
        /// Наличие кнопки для доплаты
        /// </summary>
        [DataMember]
        public int buttonBack { get; set; }

        /// <summary>
        /// Цвет
        /// </summary>
        [DataMember]
        public string color { get; set; }
    }
}
