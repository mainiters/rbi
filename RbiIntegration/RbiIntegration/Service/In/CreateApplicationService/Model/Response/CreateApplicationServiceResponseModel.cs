﻿using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.In.CreateApplicationService.Model.Response
{
    /// <summary>
    /// Модель ответа
    /// </summary>
    [DataContract]
    public class CreateApplicationServiceResponseModel : BaseResponse
    {
        /// <summary>
        /// ИД обращения
        /// </summary>
        [DataMember]
        public string TrcRequestId { get; set; }
    }
}
