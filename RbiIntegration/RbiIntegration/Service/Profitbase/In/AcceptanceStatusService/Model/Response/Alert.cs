using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.Profitbase.In.AcceptanceStatusService.Model.Response
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
        /// Цвет
        /// </summary>
        [DataMember]
        public string color { get; set; }
    }
}
