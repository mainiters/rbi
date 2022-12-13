using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.Profitbase.Out.GetFile.Model.Response
{
    /// <summary>
    /// Модель запроса
    /// </summary>
    [DataContract]
    public class GetFileServiceResponseModel : BaseResponse
    {
        /// <summary>
        /// base64Content
        /// </summary>
        [DataMember]
        public string base64Content { get; set; }

        /// <summary>
        /// fileName
        /// </summary>
        [DataMember]
        public string fileName { get; set; }

        /// <summary>
        /// name
        /// </summary>
        [DataMember]
        public string name { get; set; }

        /// <summary>
        /// originName
        /// </summary>
        [DataMember]
        public string originName { get; set; }

        /// <summary>
        /// extension
        /// </summary>
        [DataMember]
        public string extension { get; set; }

        /// <summary>
        /// createTime
        /// </summary>
        [DataMember]
        public string createTime { get; set; }

        /// <summary>
        /// size
        /// </summary>
        [DataMember]
        public string size { get; set; }

        /// <summary>
        /// id
        /// </summary>
        [DataMember]
        public string id { get; set; }

        /// <summary>
        /// typeName
        /// </summary>
        [DataMember]
        public string typeName { get; set; }

        /// <summary>
        /// documentId
        /// </summary>
        [DataMember]
        public string documentId { get; set; }
    }
}
