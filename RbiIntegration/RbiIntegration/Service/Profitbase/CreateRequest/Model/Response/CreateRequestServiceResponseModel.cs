using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.In.Profitbase.CreateRequest.Model.Response
{
    /// <summary>
    /// Модель ответа
    /// </summary>
    [DataContract]
    public class CreateRequestServiceResponseModel : BaseResponse
    {
        /// <summary>
        /// ID созданного обращения
        /// </summary>
        [DataMember]
        public string Id { get; set; }
    }
}
