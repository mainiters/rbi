using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.Profitbase.Out.BookingCreateService.Model.Response
{
    /// <summary>
    /// Модель запроса
    /// </summary>
    [DataContract]
    public class BookingCreateServiceResponseModel : BaseResponse
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
