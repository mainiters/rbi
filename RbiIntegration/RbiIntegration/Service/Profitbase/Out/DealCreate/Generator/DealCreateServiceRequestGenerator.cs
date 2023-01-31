using RbiIntegration.Service.BaseClasses;
using RbiIntegration.Service.Profitbase.Out.DealCreateService.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.Entities;

namespace RbiIntegration.Service.Profitbase.Out.DealCreateService.Generator
{
    /// <summary>
    /// Генератор запроса
    /// </summary>
    public class DealCreateServiceRequestGenerator : BaseRequestGenerator
    {
        /// <summary>
        /// Констурктор генератора
        /// </summary>
        /// <param name="userConnection">Соединение пользователя</param>
        /// <param name="serviceParams">Параметры сервиса</param>
        public DealCreateServiceRequestGenerator(UserConnection userConnection, IntegrationServiceParams serviceParams)
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
            var entity = ReadEntityData(id).First().Value;

            var res = new DealCreateServiceRequestModel()
            {
                crmDealId = entity.PrimaryColumnValue.ToString(),
                dealObject = entity.GetTypedColumnValue<string>("TrcObject_TrcObjectId"),
                payMethod = entity.GetTypedColumnValue<string>("TrcFinalFreeCalculation_TrcPaymentType_TrcProfitbaseCode"),
                typeProperty = entity.GetTypedColumnValue<string>("TrcPropertyType_TrcProfitbaseCode"),
                formDeal = entity.GetTypedColumnValue<string>("TrcRegistrationForm_TrcProfitbaseCode"),
                formEmbSign = entity.GetTypedColumnValue<string>("TrcSignIssueMethod_TrcProfitbaseCode"),
            };

            return res;
        }

        public override void AddAdditionalColumns(EntitySchemaQuery esq)
        {
            base.AddAdditionalColumns(esq);
            esq.AddColumn("TrcObject.TrcObjectId");
            esq.AddColumn("TrcFinalFreeCalculation.TrcPaymentType.TrcProfitbaseCode");
            esq.AddColumn("TrcPropertyType.TrcProfitbaseCode");
            esq.AddColumn("TrcRegistrationForm.TrcProfitbaseCode");
            esq.AddColumn("TrcRegistrationForm.TrcProfitbaseCode");
            esq.AddColumn("TrcSignIssueMethod.TrcProfitbaseCode");
        }
    }
}
