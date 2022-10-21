using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.BaseClasses
{
    /// <summary>
    /// Константы конфигурации
    /// </summary>
    public static class CrmConstants
    {
        /// <summary>
        /// Интеграционные сервисы
        /// </summary>
        public static class TrcIntegrationServices
        {
            /// <summary>
            /// Сервис проверки существования клиента
            /// </summary>
            public static Guid CheckClientExisting = new Guid("5961E49C-95DA-4115-9219-98BD8FC5A64C");

            /// <summary>
            /// Сервис создания клиента
            /// </summary>
            public static Guid CreateClient = new Guid("E2CB1F95-BC6E-44CF-B187-DA62F22C261A");

            /// <summary>
            /// Сервис создания связи клиента с помещением
            /// </summary>
            public static Guid CreateClientObjectRelation = new Guid("8838827D-1D5C-4A4D-B457-CC497AD40C3B");

            /// <summary>
            /// Сервис удаления профиля клиента
            /// </summary>
            public static Guid RemoveClientProfile = new Guid("738959BB-ED63-41E3-931D-704FDE3CFC41");

            /// <summary>
            /// Сервис удаления связи клиента с помещением
            /// </summary>
            public static Guid RemoveClientObjectRelation = new Guid("C365D1B4-1BBB-403E-8058-1A0903069EF8");

            /// <summary>
            /// Сервис добавления помещения клиенту
            /// </summary>
            public static Guid AddClientObject = new Guid("D51CA012-7F95-43C7-8971-40C29CF4D38C");

        }
    }
}
