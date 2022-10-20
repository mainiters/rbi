using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.In.RemoveClientObjectRelationService.Model.Request
{
    /// <summary>
    /// Модель запроса
    /// </summary>
    [DataContract]
    public class RemoveClientObjectRelationServiceRequestModel : BaseModel
    {
        /// <summary>
        /// ИД связи
        /// </summary>
        [DataMember]
        public string TrcConnectionObjectWithContactId { get; set; }
    }
}
