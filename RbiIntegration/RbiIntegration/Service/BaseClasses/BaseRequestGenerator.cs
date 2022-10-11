using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.Entities;

namespace RbiIntegration.Service.BaseClasses
{
    /// <summary>
    /// Базовый класс генерации запроса
    /// </summary>
    public class BaseRequestGenerator
    {
        /// <summary>
        /// Подключение пользователя
        /// </summary>
        protected UserConnection _userConnection;

        /// <summary>
        /// Параметры сервиса
        /// </summary>
        protected IntegrationServiceParams _serviceParams;

        /// <summary>
        /// Констурктор базового генератора
        /// </summary>
        /// <param name="userConnection">Соединение пользователя</param>
        /// <param name="serviceParamse">Параметры сервиса</param>
        public BaseRequestGenerator(UserConnection userConnection, IntegrationServiceParams serviceParams)
        {
            this._userConnection = userConnection;
            this._serviceParams = serviceParams;
        }

        /// <summary>
        /// Возвращает код сервиса
        /// </summary>
        /// <returns></returns>
        public virtual string GetServiceCode()
        {
            return null;
        }

        /// <summary>
        /// Получение списка дополнительных колонок для вычитки данных базовой сущности
        /// </summary>
        public virtual void AddAdditionalColumns(EntitySchemaQuery esq)
        {
        }

        /// <summary>
        /// Получение модели по идентификатору базовой сущности
        /// </summary>
        /// <param name="id">Идентификаторы базовой сущности для вычитки данных модели</param>
        /// <returns>Модель запроса</returns>
        public virtual BaseModel GenerateModel(params Guid[] id)
        {
            return new BaseModel();
        }

        /// <summary>
        /// Получение данных из БД по идентификаторам базовых сущностей
        /// </summary>
        /// <returns>Данные сущностей</returns>
        protected virtual Dictionary<Guid, Entity> ReadEntityData(params Guid[] id)
        {
            EntitySchema schema = this._userConnection.EntitySchemaManager.GetInstanceByName(this._serviceParams.EntitySchemaName);

            EntitySchemaQuery esq = new EntitySchemaQuery(schema)
            {
                UseAdminRights = true,
                CanReadUncommitedData = true,
                IgnoreDisplayValues = true
            };

            esq.AddAllSchemaColumns();

            this.AddAdditionalColumns(esq);

            esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Id", id.Select(e => e as object).ToArray()));

            var collection = esq.GetEntityCollection(this._userConnection);

            return collection.ToDictionary(e => e.GetTypedColumnValue<Guid>("Id"), e => e);
        }
    }
}
