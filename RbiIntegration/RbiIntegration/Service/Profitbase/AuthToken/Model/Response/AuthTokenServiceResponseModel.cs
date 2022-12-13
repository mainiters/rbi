using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.In.Profitbase.AuthToken.Model.Response
{
    /// <summary>
    /// Модель запроса
    /// </summary>
    [DataContract]
    public class AuthTokenServiceResponseModel : BaseResponse
    {
        /// <summary>
        /// Токен
        /// </summary>
        [DataMember]
        public string token { get; set; }
    }
}
