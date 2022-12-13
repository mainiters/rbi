using RbiIntegration.Service.BaseClasses.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Terrasoft.Core;
using Terrasoft.Core.DB;
using Terrasoft.Core.Entities;

namespace RbiIntegration.Service.BaseClasses
{
    /// <summary>
    /// Интеграционный хелпер
    /// </summary>
    public static class IntegrationServiceHelper
    {
        /// <summary>
        /// Форматирует номер телефона по маске
        /// </summary>
        /// <param name="phone">Исходный номер телефона</param>
        /// <returns></returns>
        public static string MaskPhone(string phone)
        {
            phone = CleanStringOfNonDigits(phone);

            if (phone.Length >= 10)
            {
                var lastDigits = phone.Substring(phone.Length - 10);
                return $"+7 {lastDigits[0]}{lastDigits[1]}{lastDigits[2]} {lastDigits[3]}{lastDigits[4]}{lastDigits[5]}-{lastDigits[6]}{lastDigits[7]}-{lastDigits[8]}{lastDigits[9]}";
            }

            return phone;
        }

        /// <summary>
        /// Возвращает строку с номером телефона после подготовки его к использованию в поисковом запросе
        /// </summary>
        /// <param name="phone">Исходный номер телефона</param>
        /// <returns>Номер телефона для поиска</returns>
        public static string GetReversedPhone(string phone)
        {
            phone = CleanStringOfNonDigits(phone);

            if (phone.Length == 11 && phone[0] == '8')
            {
                phone = "7" + phone.Substring(1);
            }

            return phone.CustomReverse();
        }

        /// <summary>
        /// Очистка строки от всех символов кроме цифр
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string CleanStringOfNonDigits(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }
            
            StringBuilder sb = new StringBuilder(s);
            
            int j = 0;
            int i = 0;
            
            while (i < sb.Length)
            {
                bool isDigit = char.IsDigit(sb[i]);

                if (isDigit)
                {
                    sb[j++] = sb[i++];
                }
                else
                {
                    ++i;
                }
            }

            sb.Length = j;
            
            string cleaned = sb.ToString();
            
            return cleaned;
        }

        /// <summary>
        /// Установить справочное значение с проверкой на корректный формат GUID
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="columnName"></param>
        /// <param name="columnValue"></param>
        public static void SetColumnIfGuid(Entity entity, string columnName, string columnValue)
        {
            Guid val;

            if (entity != null && string.IsNullOrWhiteSpace(columnValue) && Guid.TryParse(columnValue, out val))
            {
                entity.SetColumnValue(columnName, columnValue);
            }
        }

        /// <summary>
        /// Инициализация интеграционных сервисов
        /// </summary>
        /// <param name="userConnection"></param>
        public static void StartInServices(UserConnection userConnection)
        {

        }

        /// <summary>
        /// Получение параметров сервиса по коду
        /// </summary>
        /// <param name="serviceCode">Код сервиса</param>
        /// <returns>Параметры сервиса</returns>
        public static IntegrationServiceParams GetEntityNameByServiceCode(UserConnection userConnection, string serviceCode, string defaultUrl = null)
        {
            var entity = GetEntityByField(userConnection, "TrcIntegrationServices", "TrcServiceName", serviceCode);

            return new IntegrationServiceParams()
            {
                Id = entity.GetTypedColumnValue<Guid>("Id"),
                ServiceName = entity.GetTypedColumnValue<string>("TrcServiceName"),
                EntitySchemaName = entity.GetTypedColumnValue<string>("TrcEntitySchemaName"),
                Url = string.IsNullOrEmpty(defaultUrl) ? entity.GetTypedColumnValue<string>("TrcUrl") : defaultUrl,
                Login = entity.GetTypedColumnValue<string>("TrcLogin"),
                Password = entity.GetTypedColumnValue<string>("TrcPassword"),
                RequestType = string.IsNullOrEmpty(entity.GetTypedColumnValue<string>("TrcRequestType")) ? "POST" : entity.GetTypedColumnValue<string>("TrcRequestType"),
                Token = entity.GetTypedColumnValue<string>("TrcToken")
            };
        }

        /// <summary>
        /// Поиск справочного значения по названию
        /// </summary>
        /// <returns></returns>
        public static ExtendedEntity FindLookupItem(UserConnection userConnection, string schemaName, string fieldValue, string searchColumn = "Name", bool createIfNotFound = true, bool saveCreatedEntity = true)
        {
            if (string.IsNullOrEmpty(fieldValue))
            {
                throw new Exception($"Попытка найти пустое название в справочнике {schemaName} по колонке {searchColumn}");
            }

            ExtendedEntity entity = null;

            EntitySchema schema = userConnection.EntitySchemaManager.GetInstanceByName(schemaName);

            EntitySchemaQuery esq = new EntitySchemaQuery(schema)
            {
                UseAdminRights = true,
                CanReadUncommitedData = true,
                IgnoreDisplayValues = true
            };

            esq.AddAllSchemaColumns();

            esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, searchColumn, fieldValue));

            var collection = esq.GetEntityCollection(userConnection);

            if (collection.Count < 1)
            {
                if (createIfNotFound)
                {
                    entity = new ExtendedEntity(userConnection.EntitySchemaManager.GetInstanceByName(schemaName).CreateEntity(userConnection));

                    entity.IsJustCreated = true;

                    var entityId = Guid.NewGuid();

                    entity.Entity.SetDefColumnValues();

                    entity.Entity.SetColumnValue("Id", entityId);

                    entity.Entity.SetColumnValue(searchColumn, fieldValue);

                    if(entity.Entity.Schema.Columns.FirstOrDefault(e => e.Name == "Name") != null)
                    {
                        entity.Entity.SetColumnValue("Name", fieldValue);
                    }

                    if(saveCreatedEntity)
                    {
                        entity.Entity.Save(false, true);
                    }
                }
                else
                {
                    throw new Exception($"Не найдено значение '{fieldValue}' по колонке '{searchColumn}' в справочнике '{schemaName}'");
                }
            }
            else if (collection.Count > 1)
            {
                throw new Exception($"Найдено более одного значения '{fieldValue}' по колонке '{searchColumn}' в справочнике '{schemaName}'");
            }
            else
            {
                entity = new ExtendedEntity(collection.First());
            }

            return entity;
        }

        /// <summary>
        /// Конвертация объекта запроса в XML представление
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToXml(object data)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(data.GetType());

            using (var textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, data);
                return textWriter.ToString();
            }
        }

        public static Entity GetWebServiceUrl(UserConnection userConnection, string schemaName, string fieldValue, string searchColumn = "Name", bool createIfNotFound = true)
        {
            if (string.IsNullOrEmpty(fieldValue))
            {
                throw new Exception($"Попытка найти пустое название в справочнике {schemaName} по колонке {searchColumn}");
            }

            Entity entity = null;

            EntitySchema schema = userConnection.EntitySchemaManager.GetInstanceByName(schemaName);

            EntitySchemaQuery esq = new EntitySchemaQuery(schema)
            {
                UseAdminRights = true,
                CanReadUncommitedData = true,
                IgnoreDisplayValues = true
            };

            esq.AddAllSchemaColumns();

            esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, searchColumn, fieldValue));

            var collection = esq.GetEntityCollection(userConnection);

            if (collection.Count < 1)
            {
                if (createIfNotFound)
                {
                    entity = userConnection.EntitySchemaManager.GetInstanceByName(schemaName).CreateEntity(userConnection);

                    var entityId = Guid.NewGuid();

                    entity.SetDefColumnValues();

                    entity.SetColumnValue("Id", entityId);

                    entity.SetColumnValue(searchColumn, fieldValue);

                    if (entity.Schema.Columns.FirstOrDefault(e => e.Name == "Name") != null)
                    {
                        entity.SetColumnValue("Name", fieldValue);
                    }

                    entity.Save();
                }
                else
                {
                    throw new Exception($"Не найдено значение '{fieldValue}' по колонке '{searchColumn}' в справочнике '{schemaName}'");
                }
            }
            else if (collection.Count > 1)
            {
                throw new Exception($"Найдено более одного значения '{fieldValue}' по колонке '{searchColumn}' в справочнике '{schemaName}'");
            }
            else
            {
                entity = collection.First();
            }

            return entity;
        }


        /// <summary>
        /// Конвертация объекта запроса в JSON представление
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToJson(object data)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(data);
        }

        /// <summary>
        /// Логирование данных
        /// </summary>
        /// <param name="userConnection">Подключение пользователя</param>
        /// <param name="serviceParams">Параметры сервиса</param>
        /// <param name="uid">Уникальный идентификатор сущности</param>
        /// <param name="errorStr">Текст ошибки</param>
        /// <param name="requestStr">Запрос</param>
        /// <param name="responseStr">Ответ</param>
        public static void Log(UserConnection userConnection, IntegrationServiceParams serviceParams, DateTime requestInitDate, string title, string uid, string errorStr, string requestStr, string responseStr, int code)
        {
            var ms = (int)(DateTime.Now - requestInitDate).TotalMilliseconds;

            var insert = new Insert(userConnection)
                    .Into("TrcLogIntegration")
                    .Set("TrcName", Column.Parameter(title == null ? string.Empty : title))
                    .Set("TrcError", Column.Parameter(errorStr))
                    .Set("TrcRequest", Column.Parameter(requestStr))
                    .Set("TrcCode", Column.Parameter(code))
                    .Set("TrcRequestDuration", Column.Parameter(ms))
                    .Set("TrcResponse", Column.Parameter(responseStr));

            if (serviceParams != null)
            {
                insert = insert.Set("TrcIntegrationServicesId", Column.Parameter(serviceParams.Id));
            }

            insert.Execute();
        }

        /// <summary>
        /// Получить запись сущности по значению поля
        /// </summary>
        /// <param name="schemaName">Название схемы сущности</param>
        /// <param name="fieldName">Название поля для поиска</param>
        /// <param name="fieldValue">Значение поля для поиска</param>
        /// <returns></returns>
        public static Entity GetEntityByField(UserConnection userConnection, string schemaName, string fieldName, object fieldValue)
        {
            EntitySchema schema = userConnection.EntitySchemaManager.GetInstanceByName(schemaName);

            EntitySchemaQuery esq = new EntitySchemaQuery(schema)
            {
                UseAdminRights = true,
                CanReadUncommitedData = true,
                IgnoreDisplayValues = true
            };

            esq.AddAllSchemaColumns();

            esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, fieldName, fieldValue));

            var collection = esq.GetEntityCollection(userConnection);

            if (collection.Count > 1)
            {
                throw new EntitySearchException($"Найдено более одной сущности {schemaName} по полю {fieldName} для значения {fieldValue}");
            }
            else if(collection.Count < 1)
            {
                throw new EntitySearchException($"Не найдено сущности {schemaName} по полю {fieldName} для значения {fieldValue}");
            }

            return collection.First();
        }

        /// <summary>
        /// Вставка новой записи в таблицу БД
        /// </summary>
        /// <param name="userConnection">Подключение пользователя</param>
        /// <param name="schemaName">Название сущности</param>
        /// <param name="fileds">Коллекция названий полей и их значений</param>
        /// <returns>Созданная сущность</returns>
        public static Entity InsertEntityWithFields(UserConnection userConnection, string schemaName, Dictionary<string, object> fileds)
        {
            var entityId = Guid.NewGuid();

            var entity = userConnection.EntitySchemaManager.GetInstanceByName(schemaName).CreateEntity(userConnection);

            entity.SetDefColumnValues();

            entity.SetColumnValue("Id", entityId);

            foreach (var item in fileds)
            {
                entity.SetColumnValue(item.Key, item.Value);
            }

            entity.Save(false);

            return entity;
        }

        /// <summary>
        /// Вставка новой или обновление существующей записи в БД
        /// </summary>
        /// <param name="userConnection"></param>
        /// <param name="schemaName"></param>
        /// <param name="fileds"></param>
        /// <returns></returns>
        public static Entity InsertOrUpdateEntity(UserConnection userConnection, string schemaName, string fieldName, object fieldValue, Dictionary<string, object> fileds, Dictionary<string, object> additionalFilters = null)
        {
            Entity entity = null;

            EntitySchema schema = userConnection.EntitySchemaManager.GetInstanceByName(schemaName);

            EntitySchemaQuery esq = new EntitySchemaQuery(schema)
            {
                UseAdminRights = true,
                CanReadUncommitedData = true,
                IgnoreDisplayValues = true
            };

            esq.AddAllSchemaColumns();

            esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, fieldName, fieldValue));

            if (additionalFilters != null && additionalFilters.Count > 0)
            {
                foreach (var item in additionalFilters)
                {
                    if(item.Value != null && item.Value.ToString() == "!=null")
                    {
                        esq.Filters.Add(esq.CreateIsNotNullFilter(item.Key));
                    }
                    else if (item.Value == null)
                    {
                        esq.Filters.Add(esq.CreateIsNullFilter(item.Key));
                    }
                    else
                    {
                        esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, item.Key, item.Value));
                    }
                }
            }

            var collection = esq.GetEntityCollection(userConnection);

            if (collection.Count < 1)
            {
                entity = userConnection.EntitySchemaManager.GetInstanceByName(schemaName).CreateEntity(userConnection);

                var entityId = Guid.NewGuid();

                entity.SetDefColumnValues();

                entity.SetColumnValue("Id", entityId);
            }
            else
            {
                entity = collection.First();
            }

            foreach (var item in fileds)
            {
                entity.SetColumnValue(item.Key, item.Value);
            }

            entity.Save(false);

            return entity;
        }

        /// <summary>
        /// Вставка новой записи в БД
        /// </summary>
        /// <param name="userConnection"></param>
        /// <param name="schemaName"></param>
        /// <param name="fileds"></param>
        /// <returns></returns>
        public static Entity InsertEntity(UserConnection userConnection, string schemaName, Dictionary<string, object> fileds)
        {
            var entity = userConnection.EntitySchemaManager.GetInstanceByName(schemaName).CreateEntity(userConnection);

            var entityId = Guid.NewGuid();

            entity.SetDefColumnValues();

            entity.SetColumnValue("Id", entityId);

            foreach (var item in fileds)
            {
                entity.SetColumnValue(item.Key, item.Value);
            }

            entity.Save(false);

            return entity;
        }

        /// <summary>
        /// Функция парсинга даты из строки не зависимо от фармата
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ParseDateTime(string date)
        {
            DateTime res;

            string[] formats = 
            {
                "yyyy-MM-dd hh-mm-ss",
                "yyyy-MM-dd"
            };

            if (DateTime.TryParse(date, out res))
            {
                return res;
            }
            else if(DateTime.TryParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out res))
            {
                return res;
            }

            throw new Exception($"Не удалось преобразовать значение '{date}' к типу Дата-Время");
        }
    }
}
