using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.In.AddApplicationRatingService.Model.Request
{
    /// <summary>
    /// Модель запроса
    /// </summary>
    [DataContract]
    public class AddApplicationRatingServiceRequestModel : BaseModel
    {
        /// <summary>
        /// Id Обращения в СРМ
        /// </summary>
        [DataMember]
        public string TrcRequestId { get; set; }

        /// <summary>
        /// Рейтинг
        /// </summary>
        [DataMember]
        public string TrcRating { get; set; }

        /// <summary>
        /// Описание рейтинга
        /// </summary>
        [DataMember]
        public string TrcRatingDescription { get; set; }
    }
}
