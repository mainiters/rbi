using RbiIntegration.Service.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.Profitbase.In.ContractsManualSettService.Model.Response
{
    /// <summary>
    /// Документ для согласования клиентом
    /// </summary>
    [DataContract]
    public class AgreemDoc
    {
        /// <summary>
        /// Расположение блока на странице
        /// </summary>
        [DataMember]
        public int position { get; set; }

        /// <summary>
        /// Один файл для согласования
        /// </summary>
        [DataMember]
        public Document docs { get; set; }

        /// <summary>
        /// Расположение блока на странице
        /// </summary>
        [DataMember]
        public ConsentCheck consentCheck { get; set; }

        /// <summary>
        /// Отображение элемента для ввода комментария
        /// </summary>
        [DataMember]
        public int comment { get; set; }

        /// <summary>
        /// Отображение элемента для ввода приложения файла
        /// </summary>
        [DataMember]
        public int fileAdd { get; set; }
    }
}