using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.Profitbase.Out.Enrichment.Model.Response
{
    /// <summary>
    /// Объект с паспортными данными passDetails
    /// </summary>
    [DataContract]
    public class passDetails
    {
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
        /// Пол
        /// </summary>
        [DataMember]
        public sex sex { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        [DataMember]
        public string birthDate { get; set; }

        /// <summary>
        /// Место рождения
        /// </summary>
        [DataMember]
        public string birthPlace { get; set; }

        /// <summary>
        /// Номер паспорта
        /// </summary>
        [DataMember]
        public string numberPassport { get; set; }

        /// <summary>
        /// Серия паспорта
        /// </summary>
        [DataMember]
        public string seriesPassport { get; set; }

        /// <summary>
        /// Код подразделения
        /// </summary>
        [DataMember]
        public string issueCode { get; set; }

        /// <summary>
        /// Дата выпуска
        /// </summary>
        [DataMember]
        public string issueDate { get; set; }

        /// <summary>
        /// Файл с главной страницей паспорта
        /// </summary>
        [DataMember]
        public doc passportMain { get; set; }

        /// <summary>
        /// Файл с разворотом страницы паспорта
        /// </summary>
        [DataMember]
        public doc passportReg { get; set; }

        /// <summary>
        /// Адрес регистрации
        /// </summary>
        [DataMember]
        public adrReg adrReg { get; set; }
    }
}