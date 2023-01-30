using RbiIntegration.Service.BaseClasses;
using RbiIntegration.Service.Profitbase.Out.BookingChangeStatus.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.Entities;

namespace RbiIntegration.Service.Profitbase.Out.BookingChangeStatusService.Generator
{
    /// <summary>
    /// Генератор запроса
    /// </summary>
    public class BookingChangeStatusServiceRequestGenerator : BaseRequestGenerator
    {
        /// <summary>
        /// Констурктор генератора
        /// </summary>
        /// <param name="userConnection">Соединение пользователя</param>
        /// <param name="serviceParams">Параметры сервиса</param>
        public BookingChangeStatusServiceRequestGenerator(UserConnection userConnection, IntegrationServiceParams serviceParams)
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

            var res = new BookingChangeStatusServiceRequestModel()
            {
                documentid = entity.GetTypedColumnValue<int>("TrcProfitbaseBookingID"),
                workflowType = "ApartmentReserveProduct",
                prevStatus = "new3",
                nextStatus = "booked",
                data = new Data()
                {
                    name = entity.GetTypedColumnValue<string>("Opportunity_Contact_GivenName"),
                    surname = entity.GetTypedColumnValue<string>("Opportunity_Contact_Surname"),
                    patronymic = entity.GetTypedColumnValue<string>("Opportunity_Contact_MiddleName"),
                    phone = entity.GetTypedColumnValue<string>("Opportunity_Contact_MobilePhone"),
                    email = entity.GetTypedColumnValue<string>("Opportunity_Contact_Email"),
                    isTemporaryReserve = false,
                    isThreePerson = false,

                }
            };

            return res;
        }

        public override void AddAdditionalColumns(EntitySchemaQuery esq)
        {
            base.AddAdditionalColumns(esq);
            esq.AddColumn("Opportunity.Contact.GivenName");
            esq.AddColumn("Opportunity.Contact.Surname");
            esq.AddColumn("Opportunity.Contact.MiddleName");
            esq.AddColumn("Opportunity.Contact.MobilePhone");
            esq.AddColumn("Opportunity.Contact.Email");
        }
    }
}
