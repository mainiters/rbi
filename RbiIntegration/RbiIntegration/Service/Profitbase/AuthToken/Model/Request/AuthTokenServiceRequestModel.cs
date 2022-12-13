using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.In.Profitbase.AuthToken.Model.Request
{
    /// <summary>
    /// Модель запроса
    /// </summary>
    [DataContract]
    public class AuthTokenServiceRequestModel : BaseModel
    {
        /// <summary>
        /// Id заявки
        /// </summary>
        [DataMember]
        public string token { get; set; }
    }
}
