using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.In.Profitbase.ChangeRequestStatus.Model.Request
{
    /// <summary>
    /// Модель запроса
    /// </summary>
    [DataContract]
    public class ChangeRequestStatusServiceRequestModel : BaseModel
    {
        /// <summary>
        /// Тип запроса
        /// </summary>
        [DataMember]
        public string type { get; set; }

        /// <summary>
        /// Инфо по заявке
        /// </summary>
        [DataMember]
        public payload payload { get; set; }
    }
}
