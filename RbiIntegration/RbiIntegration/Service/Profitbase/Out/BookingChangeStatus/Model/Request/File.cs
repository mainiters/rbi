using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.Profitbase.Out.BookingChangeStatus.Model.Request
{
    /// <summary>
    /// Файл
    /// </summary>
    [DataContract]
    public class File
    {
        /// <summary>
        /// Строка в формате base64
        /// </summary>
        [DataMember]
        public string filebase64 { get; set; }

        /// <summary>
        /// название файла с расширением
        /// </summary>
        [DataMember]
        public string fileName { get; set; }
    }
}