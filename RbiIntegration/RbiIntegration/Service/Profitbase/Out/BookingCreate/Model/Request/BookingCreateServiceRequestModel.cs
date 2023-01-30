using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.Profitbase.Out.BookingCreateService.Model.Request
{
    /// <summary>
    /// Модель запроса
    /// </summary>
    [DataContract]
    public class BookingCreateServiceRequestModel : BaseModel
    {
        /// <summary>
        /// Идентификаторпродажив CRM
        /// </summary>
        [DataMember]
        public string crmDealId { get; set; }

        /// <summary>
        /// Идентификаторпомещенияв Profitbase
        /// </summary>
        [DataMember]
        public string propertyId { get; set; }

        /// <summary>
        /// Отправитьссылкудлявходапо sms
        /// </summary>
        [DataMember]
        public bool sendNotificationOnSms { get; set; }

        /// <summary>
        /// Отправитьссылкудлявходапо email
        /// </summary>
        [DataMember]
        public bool sendNotificationOnEmail { get; set; }
    }
}
