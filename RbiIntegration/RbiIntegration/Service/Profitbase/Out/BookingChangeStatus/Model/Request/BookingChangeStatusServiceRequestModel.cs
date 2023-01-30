using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.Profitbase.Out.BookingChangeStatus.Model.Request
{
    /// <summary>
    /// Модель запроса
    /// </summary>
    [DataContract]
    public class BookingChangeStatusServiceRequestModel : BaseModel
    {
        /// <summary>
        /// id заявки в ЛК
        /// </summary>
        [DataMember]
        public int documentid { get; set; }

        /// <summary>
        /// Тип заявки
        /// </summary>
        [DataMember]
        public string workflowType { get; set; }

        /// <summary>
        /// Текущий статус заявки, из которого должен быть совершен переход
        /// </summary>
        [DataMember]
        public string prevStatus { get; set; }

        public bool ShouldSerializeprevStatus()
        {
            return !string.IsNullOrEmpty(this.prevStatus);
        }

        /// <summary>
        /// Статус, в который перешла заявка
        /// </summary>
        [DataMember]
        public string nextStatus { get; set; }

        /// <summary>
        /// Массив обязательных данных, которые требуются для осуществления перехода
        /// </summary>
        [DataMember]
        public Data data { get; set; }
    }
}
