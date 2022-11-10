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
    /// Данные файла
    /// </summary>
    [DataContract]
    public class File
    {
        /// <summary>
        /// Ссылка на скачивание
        /// </summary>
        [DataMember]
        public string Link { get; set; }

        /// <summary>
        /// Имя файла
        /// </summary>
        [DataMember]
        public string FileName { get; set; }
    }
}
