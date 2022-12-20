using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.BaseClasses
{
    /// <summary>
    /// Базовый класс ответа интеграционного сервиса (ответа от СРМ)
    /// </summary>
    [DataContract]
    public class BaseResponse
    {
        /// <summary>
        /// Признак успешной обработки
        /// </summary>
        [DataMember]
        public bool Result { get; set; }

        /// <summary>
        /// Код ответа
        /// </summary>
        public int Code
        {
            get
            {
                return Result ? 200 : 500;
            }
            set
            {
                code = value;
            }
        }

        public bool ShouldSerializeCode()
        {
            return false;
        }

        protected int code { get; set; }


        /// <summary>
        /// Сообщение
        /// </summary>
        [DataMember]
        public string Message { get; set; }


        /// <summary>
        /// Описание
        /// </summary>
        public string ReasonPhrase
        {
            get
            {
                return reasonPhrase;
            }

            set
            {
                reasonPhrase = value;
                Exception = value;
            }
        }

        public bool ShouldSerializeReasonPhrase()
        {
            return false;
        }

        protected string reasonPhrase;

        /// <summary>
        /// Ошибка
        /// </summary>
        [DataMember]
        public string Exception { get; set; }

        /// <summary>
        /// Детали исключения
        /// </summary>
        [DataMember]
        public string StackTrace { get; set; }

        /// <summary>
        /// Детали исключения
        /// </summary>
        [DataMember]
        public object ServiceResponse { get; set; }

        public BaseResponse()
        {
            this.Code = 200;
            this.Message = "Ok";
            this.Exception = string.Empty;
            this.StackTrace = string.Empty;
            this.Result = true;
        }
    }
}
