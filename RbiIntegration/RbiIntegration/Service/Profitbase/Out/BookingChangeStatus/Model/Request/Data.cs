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
    /// Массив обязательных данных, которые требуются для осуществления перехода
    /// </summary>
    [DataContract]
    public class Data
    {
        /// <summary>
        /// Имя
        /// </summary>
        [DataMember]
        public string name { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [DataMember]
        public string surname { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        [DataMember]
        public string patronymic { get; set; }

        public bool ShouldSerializepatronymic()
        {
            return !string.IsNullOrEmpty(this.patronymic);
        }

        /// <summary>
        /// телефон
        /// </summary>
        [DataMember]
        public string phone { get; set; }

        /// <summary>
        /// email
        /// </summary>
        [DataMember]
        public string email { get; set; }

        /// <summary>
        /// 3-e лицо
        /// </summary>
        [DataMember]
        public bool isThreePerson { get; set; }

        /// <summary>
        /// Признак бессрочной брони
        /// </summary>
        [DataMember]
        public bool isTemporaryReserve { get; set; }

        /// <summary>
        /// Дата окончания брони в UTC
        /// </summary>
        [DataMember]
        public string endBookingDateUTC { get; set; }

        public bool ShouldSerializeendBookingDateUTC()
        {
            return !string.IsNullOrEmpty(this.endBookingDateUTC);
        }

        /// <summary>
        /// Платная ли бронь
        /// </summary>
        [DataMember]
        public bool isPayBooking { get; set; }

        /// <summary>
        /// файлы Фото разворота паспорта
        /// </summary>
        [DataMember]
        public List<File> filesPassport { get; set; }

        public bool ShouldSerializefilesPassport()
        {
            return this.filesPassport != null && this.filesPassport.Count > 0;
        }

        /// <summary>
        /// Серия паспорта
        /// </summary>
        [DataMember]
        public string seriesPassport { get; set; }

        public bool ShouldSerializeseriesPassport()
        {
            return !string.IsNullOrEmpty(this.seriesPassport);
        }
        /// <summary>
        /// Номер паспорта
        /// </summary>
        [DataMember]
        public string numberPassport { get; set; }

        /// <summary>
        /// Дата выдачи
        /// </summary>
        [DataMember]
        public string issueDate { get; set; }

        public bool ShouldSerializeissueDate()
        {
            return !string.IsNullOrEmpty(this.issueDate);
        }

        /// <summary>
        /// Код подразделения
        /// </summary>
        [DataMember]
        public string issueCode { get; set; }

        public bool ShouldSerializeissueCode()
        {
            return !string.IsNullOrEmpty(this.issueCode);
        }

        /// <summary>
        /// ИНН
        /// </summary>
        [DataMember]
        public string inn { get; set; }

        public bool ShouldSerializeinn()
        {
            return !string.IsNullOrEmpty(this.inn);
        }

        /// <summary>
        /// СНИЛС
        /// </summary>
        [DataMember]
        public string snils { get; set; }

        public bool ShouldSerializesnils()
        {
            return !string.IsNullOrEmpty(this.snils);
        }

        /// <summary>
        /// Адрес регистрации
        /// </summary>
        [DataMember]
        public string addressReg { get; set; }

        public bool ShouldSerializeaddressReg()
        {
            return !string.IsNullOrEmpty(this.addressReg);
        }

    }
}