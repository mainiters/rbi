using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.In.AddClientObjectService.Model.Response
{
    /// <summary>
    /// Модель ответа
    /// </summary>
    [DataContract]
    public class AddClientObjectServiceResponseModel : BaseResponse
    {
        /// <summary>
        /// ИД связи
        /// </summary>
        [DataMember]
        public string TrcConnectionObjectWithContactId { get; set; }
    }
}
