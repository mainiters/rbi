using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.In.CheckClientExistingService.Model.Request
{
    /// <summary>
    /// Модель проверки существования клиента
    /// </summary>
    [DataContract]
    public class CheckClientExistingServiceRequestModel : BaseModel
    {
        /// <summary>
        /// Телефоны
        /// </summary>
        [DataMember]
        public PhoneData[] Phones { get; set; }
    }
}
