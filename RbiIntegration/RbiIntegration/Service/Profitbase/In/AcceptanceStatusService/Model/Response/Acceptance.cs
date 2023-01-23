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
    /// Информация о доступной приемке
    /// </summary>
    [DataContract]
    public class Acceptance
    {
        /// <summary>
        /// Расположение блока на странице
        /// </summary>
        [DataMember]
        public int position { get; set; }

        /// <summary>
        /// Доступна возможность выбора даты/времени
        /// </summary>
        [DataMember]
        public int? option { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        [DataMember]
        public string date { get; set; }

        /// <summary>
        /// Времяы
        /// </summary>
        [DataMember]
        public string time { get; set; }
    }
}