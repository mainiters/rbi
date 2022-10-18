using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.In.CheckClientExistingService.Model.Response
{
    /// <summary>
    /// Модель ответа
    /// </summary>
    [DataContract]
    public class CheckClientExistingServiceResponseModel : BaseResponse
    {
        /// <summary>
        /// ИД контакта
        /// </summary>
        [DataMember]
        public string TrcContactId { get; set; }
    }
}
