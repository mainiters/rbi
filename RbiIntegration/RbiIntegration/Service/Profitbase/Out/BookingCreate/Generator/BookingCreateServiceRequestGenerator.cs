using RbiIntegration.Service.BaseClasses;
using RbiIntegration.Service.Profitbase.Out.BookingCreateService.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.Entities;

namespace RbiIntegration.Service.Profitbase.Out.BookingCreateService.Generator
{
    /// <summary>
    /// Генератор запроса
    /// </summary>
    public class BookingCreateServiceRequestGenerator : BaseRequestGenerator
    {
        /// <summary>
        /// Констурктор генератора
        /// </summary>
        /// <param name="userConnection">Соединение пользователя</param>
        /// <param name="serviceParams">Параметры сервиса</param>
        public BookingCreateServiceRequestGenerator(UserConnection userConnection, IntegrationServiceParams serviceParams)
            : base(userConnection, serviceParams)
        {

        }

        /// <summary>
        /// Генерация модели запроса
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override BaseModel GenerateModel(params string[] id)
        {
            var res = new BookingCreateServiceRequestModel();

            var data = ReadEntityData(id);

            res.crmDealId = id.ToString();
            res.propertyId = data.First().Value.GetTypedColumnValue<string>("TrcObject_TrcObjectId");
            res.sendNotificationOnSms = data.First().Value.GetTypedColumnValue<bool>("TrcSendNotificationOnSms");
            res.sendNotificationOnEmail = data.First().Value.GetTypedColumnValue<bool>("TrcSendNotificationOnEmail");

            return res;
        }

        public override void AddAdditionalColumns(EntitySchemaQuery esq)
        {
            base.AddAdditionalColumns(esq);
            esq.AddColumn("TrcObject.TrcObjectId");
        }
    }
}
