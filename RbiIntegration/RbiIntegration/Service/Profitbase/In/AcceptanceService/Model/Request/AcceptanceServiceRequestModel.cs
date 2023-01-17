using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.Profitbase.In.AcceptanceService.Model.Request
{
    /// <summary>
    /// Модель запроса
    /// </summary>
    [DataContract]
    public class AcceptanceServiceRequestModel : BaseModel
    {
        /// <summary>
        /// тип события, список поддерживаемых событий
        /// </summary>
        [DataMember]
        public string type { get; set; }

        /// <summary>
        /// Объект, содержащий данные события
        /// </summary>
        [DataMember]
        public Payload payload { get; set; }
    }
}
