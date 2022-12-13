using RbiIntegration.Service.BaseClasses;
using RbiIntegration.Service.In.Profitbase.Enrichment.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.Entities;

namespace RbiIntegration.Service.Profitbase.Enrichment.Handler
{
    /// <summary>
    /// Обработчик ответа сервиса
    /// </summary>
    public class EnrichmentServiceResponseHandler : BaseResponseHandler
    {
        /// <summary>
        /// Констурктор обработчика ответа сервиса
        /// </summary>
        /// <param name="userConnection">Соединение пользователя</param>
        /// <param name="serviceParams">Параметры интеграционного сервиса</param>
        public EnrichmentServiceResponseHandler(UserConnection userConnection, IntegrationServiceParams serviceParams)
            : base(userConnection, serviceParams)
        {

        }

        public override void Handle(BaseResponse response, params Guid[] id)
        {
            var requestId = id.First();
            Entity request = IntegrationServiceHelper.GetEntityByField(this._userConnection, "TrcRequest", "Id", requestId);
            Entity contact = IntegrationServiceHelper.GetEntityByField(this._userConnection, "Contact", "Id", request.GetTypedColumnValue<Guid>("TrcContactId"));

            var responseModel = response as EnrichmentServiceResponseModel;

            if (!contact.GetTypedColumnValue<bool>("TrcIsEnriched"))
            {
                if (responseModel.passDetails != null)
                {
                    contact.SetColumnValue("TrcBirthDate", DateTime.Parse(responseModel.passDetails.birthDate));

                    if (responseModel.passDetails.adrReg != null && !string.IsNullOrEmpty(responseModel.passDetails.adrReg.fullAddress))
                    {
                        contact.SetColumnValue("TrcRegistrationAddress", responseModel.passDetails.adrReg.fullAddress);

                        IntegrationServiceHelper.InsertEntityWithFields(this._userConnection, "ContactAddress", new Dictionary<string, object>()
                        {
                            { "ContactId", contact.PrimaryColumnValue },
                            { "Address", responseModel.passDetails.adrReg.fullAddress },
                            { "AddressTypeId", Guid.Parse("7E40A853-06B8-4856-9373-3B966C7153B5") }
                        });

                        IntegrationServiceHelper.InsertEntityWithFields(this._userConnection, "TrcPaymentDetails", new Dictionary<string, object>()
                        {
                            { "TrcContactId", contact.PrimaryColumnValue },
                            { "TrcNumber", responseModel.passDetails.numberPassport },
                            { "TrcSeries", responseModel.passDetails.seriesPassport },
                            { "TrcDivisionCode", responseModel.passDetails.issueCode },
                            { "TrcDate", DateTime.Parse(responseModel.passDetails.issueDate) },
                            { "TrcPlaceOfBirth", responseModel.passDetails.birthPlace }
                        });
                    }

                    contact.SetColumnValue("TrcIsEnriched", true);

                    contact.Save(false);
                }

                if (!string.IsNullOrEmpty(responseModel.typeServ))
                {
                    try
                    {
                        var service = IntegrationServiceHelper.GetEntityByField(this._userConnection, "TrcService", "TrcIdLKService", responseModel.typeServ);
                        request.SetColumnValue("TrcServiceId", service.PrimaryColumnValue);
                    }
                    catch (Exception)
                    {
                        // Услуга не найдена
                        request.SetColumnValue("TrcDescription", "Для уточнения услуги свяжитесь с клиентом");
                    }
                    request.Save(false);
                }

                if (responseModel.freeForm != null)
                {
                    IntegrationServiceHelper.InsertEntityWithFields(this._userConnection, "TrcMessagesFromPersonalAccount", new Dictionary<string, object>()
                    {
                        { "TrcRequestId", request.PrimaryColumnValue },
                        { "TrcAnswer", responseModel.freeForm.comment },
                        { "TrcResponseDate", DateTime.Now }
                    });
                }
            }
            
            if (responseModel.infoApproval != null)
            {
                if (responseModel.infoApproval.solution.HasValue)
                {
                    request.SetColumnValue("TrcAgreed", responseModel.infoApproval.solution.Value);
                }

                request.SetColumnValue("TrcRejectionReason", responseModel.infoApproval.comment);

                request.Save(false);
            }
        }
    }
}
