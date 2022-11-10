using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.In.AddFileService.Model.Request
{
    /// <summary>
    /// Модель запроса
    /// </summary>
    [DataContract]
    public class AddFileServiceRequestModel : BaseModel
    {
        /// <summary>
        /// ИД Обращения в CRM
        /// </summary>
        [DataMember]
        public string TrcRequestId { get; set; }

        /// <summary>
        /// Массив файлов
        /// </summary>
        [DataMember]
        public File[] File { get; set; }

    }
}
