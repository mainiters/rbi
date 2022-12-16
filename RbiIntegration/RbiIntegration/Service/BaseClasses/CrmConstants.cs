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

            /// <summary>
            /// Сервис создания заявки
            /// </summary>
            public static Guid CreateApplication = new Guid("65176FF1-C66B-47B4-8E2B-449DD93A2414");

            /// <summary>
            /// Сервис обновления заявки
            /// </summary>
            public static Guid UpdateApplication = new Guid("3F54F434-93AC-4739-A453-2F8E0AA5D8B1");

            /// <summary>
            /// Добавление оценки для заявки
            /// </summary>
            public static Guid AddApplicationRating = new Guid("2DDAE9E1-50F8-4EAE-887D-0200DF13B755");

            /// <summary>
            /// Добавление файлов
            /// </summary>
            public static Guid AddFile = new Guid("315D0251-4C55-4B9F-BAC8-2232DFFBCB35");

            /// <summary>
            /// Сервис создания заявки
            /// </summary>
            public static Guid Request = new Guid("00586EA7-D714-40EB-AF4F-A82FFB8090FC");

            /// <summary>
            /// Сервис изменения статуса заявки
            /// </summary>
            public static Guid ChangeRequestStatus = new Guid("E88E55E7-DB97-4C4F-8A2C-95A78ECD9E30");

        }
    }
}
