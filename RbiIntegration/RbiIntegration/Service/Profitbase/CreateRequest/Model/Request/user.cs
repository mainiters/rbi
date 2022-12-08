using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.In.Profitbase.CreateRequest.Model.Request
{
    /// <summary>
    /// Информация о пользователе
    /// </summary>
    public class user
    {
        /// <summary>
        /// id пользователя в ЛК
        /// </summary>
        [DataMember]
        public string userId { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [DataMember]
        public string name { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        [DataMember]
        public string patronymic { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [DataMember]
        public string surname { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        [DataMember]
        public string phonenumber { get; set; }

        /// <summary>
        /// Почта
        /// </summary>
        [DataMember]
        public string email { get; set; }
    }
}