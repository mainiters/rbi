using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.In.CreateClientService.Model.Response
{
    /// <summary>
    /// Модель ответа
    /// </summary>
    [DataContract]
    public class CreateClientServiceResponseModel : BaseResponse
    {
        /// <summary>
        /// ИД контакта
        /// </summary>
        [DataMember]
        public string TrcContactId { get; set; }
    }
}
