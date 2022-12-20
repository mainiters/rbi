using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.Profitbase.In.RequestService.Model.Response
{
    /// <summary>
    /// Модель ответа
    /// </summary>
    [DataContract]
    public class RequestServiceResponseModel : BaseResponse
    {
        /// <summary>
        /// ID созданного обращения
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public new bool Result { get; set; }

        /// <summary>
        /// Код ответа
        /// </summary>
        [DataMember]
        public new int Code { get; set; }
    }
}
