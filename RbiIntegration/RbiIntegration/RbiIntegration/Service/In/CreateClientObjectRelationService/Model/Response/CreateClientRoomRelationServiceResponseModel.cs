using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.In.CreateClientObjectRelationService.Model.Response
{
    /// <summary>
    /// Модель ответа
    /// </summary>
    [DataContract]
    public class CreateClientObjectRelationServiceResponseModel : BaseResponse
    {
        /// <summary>
        /// ИД связи
        /// </summary>
        [DataMember]
        public string TrcConnectionObjectWithContactId { get; set; }
    }
}
