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
    /// Адрес регистрации
    /// </summary>
    [DataContract]
    public class adrReg
    {
        /// <summary>
        /// Нет квартиры/офиса
        /// </summary>
        [DataMember]
        public bool? noFlat { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        [DataMember]
        public string fullAddress { get; set; }

        /// <summary>
        /// Регион/район
        /// </summary>
        [DataMember]
        public string region { get; set; }

        /// <summary>
        /// Тип района
        /// </summary>
        [DataMember]
        public string areaType { get; set; }

        /// <summary>
        /// Наименование района
        /// </summary>
        [DataMember]
        public string area { get; set; }

        /// <summary>
        /// Тип города
        /// </summary>
        [DataMember]
        public string cityType { get; set; }

        /// <summary>
        /// Наименование города
        /// </summary>
        [DataMember]
        public string city { get; set; }

        /// <summary>
        /// Тип населенного пункта
        /// </summary>
        [DataMember]
        public string settlementType { get; set; }

        /// <summary>
        /// Наименование населенного пункта
        /// </summary>
        [DataMember]
        public string settlement { get; set; }

        /// <summary>
        /// Город/Населенный пункт
        /// </summary>
        [DataMember]
        public string settlementAndCity { get; set; }

        /// <summary>
        /// Тип улицы
        /// </summary>
        [DataMember]
        public string streetType { get; set; }

        /// <summary>
        /// Наименование улицы
        /// </summary>
        [DataMember]
        public string street { get; set; }

        /// <summary>
        /// Улица
        /// </summary>
        [DataMember]
        public string streetWithType { get; set; }

        /// <summary>
        /// Тип дома
        /// </summary>
        [DataMember]
        public string buildings { get; set; }

        /// <summary>
        /// Номер дома
        /// </summary>
        [DataMember]
        public string buildingNumbers { get; set; }

        /// <summary>
        /// Тип корпуса
        /// </summary>
        [DataMember]
        public string block { get; set; }

        /// <summary>
        /// Номер корпуса
        /// </summary>
        [DataMember]
        public string blockNumber { get; set; }

        /// <summary>
        /// Тип квартиры
        /// </summary>
        [DataMember]
        public string[] flats { get; set; }

        /// <summary>
        /// Номер квартиры
        /// </summary>
        [DataMember]
        public string[] flatNumbers { get; set; }

        /// <summary>
        /// Индекс
        /// </summary>
        [DataMember]
        public string index { get; set; }

        /// <summary>
        /// Код субъекта РФ
        /// </summary>
        [DataMember]
        public string code { get; set; }

        /// <summary>
        /// Код ИФНС
        /// </summary>
        [DataMember]
        public string taxOffice { get; set; }

        /// <summary>
        /// ISO код страны
        /// </summary>
        [DataMember]
        public string countryISO { get; set; }

        /// <summary>
        /// ISO код региона
        /// </summary>
        [DataMember]
        public string regionISO { get; set; }

        /// <summary>
        /// ФИАС региона
        /// </summary>
        [DataMember]
        public string regionFIAS { get; set; }

        /// <summary>
        /// ФИАС района
        /// </summary>
        [DataMember]
        public string areaFias { get; set; }

        /// <summary>
        /// ФИАС города
        /// </summary>
        [DataMember]
        public string cityFIAS { get; set; }

        /// <summary>
        /// ФИАС населенного пункта
        /// </summary>
        [DataMember]
        public string settlementFIAS { get; set; }

        /// <summary>
        /// ФИАС код улицы
        /// </summary>
        [DataMember]
        public string streetFIAS { get; set; }

        /// <summary>
        /// Код ОКТМО
        /// </summary>
        [DataMember]
        public string oktmo { get; set; }

        /// <summary>
        /// Код ФИАС
        /// </summary>
        [DataMember]
        public string fiasCode { get; set; }

        /// <summary>
        /// subareaType
        /// </summary>
        [DataMember]
        public string subareaType { get; set; }

        /// <summary>
        /// subarea
        /// </summary>
        [DataMember]
        public string subarea { get; set; }

        /// <summary>
        /// areaCode
        /// </summary>
        [DataMember]
        public string areaCode { get; set; }

        /// <summary>
        /// settlementCode
        /// </summary>
        [DataMember]
        public string settlementCode { get; set; }

        /// <summary>
        /// Наименование планировочной структуры
        /// </summary>
        [DataMember]
        public string planningStructure { get; set; }

        /// <summary>
        /// Тип планировочной структуры
        /// </summary>
        [DataMember]
        public string planningStructureType { get; set; }

    }
}