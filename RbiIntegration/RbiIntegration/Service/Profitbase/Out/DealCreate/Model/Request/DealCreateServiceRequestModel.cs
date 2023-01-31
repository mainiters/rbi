using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.Profitbase.Out.DealCreateService.Model.Request
{
    /// <summary>
    /// Модель запроса
    /// </summary>
    [DataContract]
    public class DealCreateServiceRequestModel : BaseModel
    {
        /// <summary>
        /// ID сделки в CRM
        /// </summary>
        [DataMember]
        public string crmDealId { get; set; }

        /// <summary>
        /// ID помещения
        /// </summary>
        [DataMember]
        public string dealObject { get; set; }

        /// <summary>
        /// ID способа оплаты  
        /// </summary>
        [DataMember]
        public string payMethod { get; set; }

        /// <summary>
        /// ID Типа собственности
        /// </summary>
        [DataMember]
        public string typeProperty { get; set; }

        /// <summary>
        /// ID Способа регистрации
        /// </summary>
        [DataMember]
        public string formDeal { get; set; }

        /// <summary>
        /// ID Способа выпуска ЭЦП
        /// </summary>
        [DataMember]
        public string formEmbSign { get; set; }
    }
}
