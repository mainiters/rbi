using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.Profitbase.Out.GetFile.Model.Request
{
    /// <summary>
    /// Модель запроса
    /// </summary>
    [DataContract]
    public class GetFileServiceRequestModel : BaseModel
    {
        /// <summary>
        /// Id файла
        /// </summary>
        [DataMember]
        public string fileId { get; set; }
    }
}
