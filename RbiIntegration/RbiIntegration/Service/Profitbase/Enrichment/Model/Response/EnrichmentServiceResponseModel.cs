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
    /// Модель запроса
    /// </summary>
    [DataContract]
    public class EnrichmentServiceResponseModel : BaseResponse
    {
        /// <summary>
        /// Номер договора
        /// </summary>
        [DataMember]
        public string conttractNumber { get; set; }

        /// <summary>
        /// Идентификатор услуги ЛК
        /// </summary>
        [DataMember]
        public string typeServ { get; set; }

        /// <summary>
        /// Информация со свободной формой
        /// </summary>
        [DataMember]
        public freeForm freeForm { get; set; }

        /// <summary>
        /// Информация с паспортными данными
        /// </summary>
        [DataMember]
        public passDetails passDetails { get; set; }

        /// <summary>
        /// Информация с банковскими реквизитами
        /// </summary>
        [DataMember]
        public bankDetails bankDetails { get; set; }

        /// <summary>
        /// Информация о согласовании
        /// </summary>
        [DataMember]
        public infoApproval infoApproval { get; set; }

        /// <summary>
        /// Информация о необходимости доработки
        /// </summary>
        [DataMember]
        public infoRevision infoRevision { get; set; }


    }
}
