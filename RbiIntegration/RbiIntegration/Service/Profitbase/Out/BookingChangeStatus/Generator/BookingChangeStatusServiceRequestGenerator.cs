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

            var days = 0;
            var isPayBooking = false;

            if (entity.GetTypedColumnValue<Guid>("StageId") == Guid.Parse("89EFB715-CB95-49A3-9EC3-B8B8BBFC578D"))
            {
                days = 1;
            }
            else if(entity.GetTypedColumnValue<Guid>("StageId") == Guid.Parse("1921AAA2-1584-4BC9-B8E4-8336CB4D39C0"))
            {
                days = 5;
                isPayBooking = true;
            }

            var processRes = new Dictionary<string, object>()
            {
                { "ProcessSchemaParameter1", 0 }
            };

            ProcessHelper.RunProcess(this._userConnection, "TrcCalculationOfWorkingDay", new Dictionary<string, object>()
            {
                { "Day", days }
            }, processRes);

            var endBookingDateUTC = DateTime.UtcNow.AddDays((double)processRes.First().Value);

            var esq = new EntitySchemaQuery(this._userConnection.EntitySchemaManager, "TrcPaymentDetails");

            esq.AddAllSchemaColumns();

            esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcContact", entity.GetTypedColumnValue<Guid>("ContactId")));

            var paymentDetails = esq.GetEntityCollection(this._userConnection);

            var paymentDetailsPassport = paymentDetails.Where(e => e.GetTypedColumnValue<Guid>("TrcDocumentId") == Guid.Parse("B521130D-7267-4E12-862F-689F5B0795DA")).FirstOrDefault();
            var paymentDetailsInn = paymentDetails.Where(e => e.GetTypedColumnValue<Guid>("TrcDocumentId") == Guid.Parse("CFB375D3-0AC5-4BD6-ADA7-592D7F5AA8C6")).FirstOrDefault();
            var paymentDetailsSnils = paymentDetails.Where(e => e.GetTypedColumnValue<Guid>("TrcDocumentId") == Guid.Parse("4DF474DB-7703-4341-9C7F-621C42317C71")).FirstOrDefault();

            esq = new EntitySchemaQuery(this._userConnection.EntitySchemaManager, "ContactAddress");

            esq.AddAllSchemaColumns();

            esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "Contact", entity.GetTypedColumnValue<Guid>("ContactId")));
            esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "AddressType", Guid.Parse("7E40A853-06B8-4856-9373-3B966C7153B5")));

            var address = esq.GetEntityCollection(this._userConnection).FirstOrDefault();

            esq = new EntitySchemaQuery(this._userConnection.EntitySchemaManager, "TrcPaymentDetailsFile");

            esq.AddAllSchemaColumns();

            esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcContact", entity.GetTypedColumnValue<Guid>("ContactId")));

            var files = esq.GetEntityCollection(this._userConnection);

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
                    isThreePerson = false,
                    isTemporaryReserve = false,
                    endBookingDateUTC = endBookingDateUTC.ToString(),
                    isPayBooking = isPayBooking,
                    numberPassport = paymentDetailsPassport?.GetTypedColumnValue<string>("TrcNumber"),
                    issueDate = paymentDetailsPassport?.GetTypedColumnValue<string>("TrcIssueDate"),
                    issueCode = paymentDetailsPassport?.GetTypedColumnValue<string>("TrcDivisionCode"),
                    inn = paymentDetailsInn?.GetTypedColumnValue<string>("TrcNumber"),
                    snils = paymentDetailsSnils?.GetTypedColumnValue<string>("TrcNumber"),
                    addressReg = address?.GetTypedColumnValue<string>("Address")
                }
            };

            if (files != null && files.Count > 0)
            {
                res.data.filesPassport = new List<File>();

                foreach (var item in files)
                {
                    res.data.filesPassport.Add(new File()
                    {
                        fileName = item.GetTypedColumnValue<string>("Name"),
                        filebase64 = Convert.ToBase64String(item.GetColumnValue("Data") as byte[])
                    });
                }
            }

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
