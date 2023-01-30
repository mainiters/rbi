using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.Profitbase.Out.BookingChangeStatusService.Model.Response
{
    /// <summary>
    /// Модель ответа
    /// </summary>
    [DataContract]
    public class BookingChangeStatusServiceResponseModel : BaseResponse
    {
        /// <summary>
        /// id - созданной заявки
        /// </summary>
        [DataMember]
        public string documentId { get; set; }

        /// <summary>
        /// ссылка для редиректа пользователя настраницу заявки
        /// </summary>
        [DataMember]
        public string lkUrl { get; set; }
    }
}
