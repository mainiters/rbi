using RbiIntegration.Service.BaseClasses;
using RbiIntegration.Service.In.CheckClientExistingService.Model;
using RbiIntegration.Service.In.CheckClientExistingService.Model.Request;
using RbiIntegration.Service.In.CheckClientExistingService.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core.Entities;
using Terrasoft.Web.Common;
using static RbiIntegration.Service.BaseClasses.CrmConstants;
using RbiIntegration.Service.BaseClasses.Extensions;

namespace RbiIntegration.Service.In.CheckClientExistingService
{
    /// <summary>
    /// Сервис проверки существования клиента
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class CheckClientExistingService : BaseRbiService<CheckClientExistingServiceRequestModel, CheckClientExistingServiceResponseModel>
    {
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        protected override CheckClientExistingServiceResponseModel ProcessBusinessLogic(CheckClientExistingServiceRequestModel requestModel, CheckClientExistingServiceResponseModel response)
        {
            if (requestModel.Phones == null || requestModel.Phones.Length < 1 || requestModel.Phones.Count(e => e.Basic == true) < 1)
            {
                throw new Exception("В запросе отстутствует основной номер телефона");
            }

            var phone = requestModel.Phones.First(e => e.Basic == true).Phone;

            var reversedPhone = IntegrationServiceHelper.GetReversedPhone(phone);

            var esq = new EntitySchemaQuery(this.UserConnection.EntitySchemaManager, "ContactCommunication");

            esq.AddColumn("Contact");

            esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "SearchNumber", reversedPhone));

            var entities = esq.GetEntityCollection(this.UserConnection);

            if (entities.Count < 1)
            {
                response.Result = false;
                response.Code = 104001;
                response.ReasonPhrase = $"Контакт с номером {phone} не найден";
            }
            else if (entities.Count > 1)
            {
                response.Result = false;
                response.Code = 104001;
                response.ReasonPhrase = $"Найдено более одного контакта с номером {phone}";
            }
            else
            {
                response.TrcContactId = entities.First().GetTypedColumnValue<string>("ContactId");
            }

            return response;
        }

        protected override void InitRequiredFields(List<string> requiredFields)
        {
            requiredFields.Add("Phones");
        }

        protected override void CheckRequiredFields(CheckClientExistingServiceRequestModel request, CheckClientExistingServiceResponseModel response)
        {
            base.CheckRequiredFields(request, response);

            if (response.Result)
            {
                foreach (var item in request.Phones)
                {
                    if (item.Basic == null)
                    {
                        response.ReasonPhrase = $"Обязательное поле Basic не заполнено";
                        response.Code = 304001;
                        return;
                    }

                    if (string.IsNullOrEmpty(item.Phone))
                    {
                        response.ReasonPhrase = $"Обязательное поле Phone не заполнено";
                        response.Code = 304001;
                        return;
                    }
                }
            }
        }
    }
}
