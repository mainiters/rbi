using RbiIntegration.Service.BaseClasses;
using RbiIntegration.Service.Profitbase.Out.AuthToken.Model.Response;
using RbiIntegration.Service.Profitbase.Out.Enrichment.Model.Response;
using RbiIntegration.Service.Profitbase.Out.BookingCreateService.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.DB;
using Terrasoft.Core.Entities;

namespace RbiIntegration.Service.Profitbase.Out.BookingCreateService.Handler
{
    /// <summary>
    /// Обработчик ответа сервиса
    /// </summary>
    public class BookingCreateServiceResponseHandler : BaseResponseHandler
    {
        /// <summary>
        /// Констурктор обработчика ответа сервиса
        /// </summary>
        /// <param name="userConnection">Соединение пользователя</param>
        /// <param name="serviceParams">Параметры интеграционного сервиса</param>
        public BookingCreateServiceResponseHandler(UserConnection userConnection, IntegrationServiceParams serviceParams)
            : base(userConnection, serviceParams)
        {

        }

        public override void Handle(BaseResponse response, params string[] id)
        {
            var responseModel = response as BookingCreateServiceResponseModel;

            EntitySchema schema = this._userConnection.EntitySchemaManager.GetInstanceByName(this._serviceParams.EntitySchemaName);

            EntitySchemaQuery esq = new EntitySchemaQuery(schema)
            {
                UseAdminRights = true,
                CanReadUncommitedData = true,
                IgnoreDisplayValues = true
            };

            esq.AddAllSchemaColumns();

            var entity = esq.GetEntity(this._userConnection, id.First());

            entity.SetColumnValue("TrcProfitbaseBookingId", responseModel.documentId);
            entity.SetColumnValue("rcProfitbaseBookingDealLink", responseModel.lkUrl);

            entity.Save(false);
        }
    }
}
