using RbiIntegration.Service.BaseClasses;
using RbiIntegration.Service.Profitbase.In.ContractsListService.Model.Request;
using RbiIntegration.Service.Profitbase.In.ContractsListService.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.Entities;
using Terrasoft.Web.Common;
using static RbiIntegration.Service.BaseClasses.CrmConstants;

namespace RbiIntegration.Service.Profitbase.In.ContractsListService
{
    /// <summary>
    /// Сервис получения списка доступных договоров
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class ContractsListService : BaseRbiService<ContractsListServiceRequestModel, ContractsListServiceResponseModel>
    {
        public ContractsListService(UserConnection UserConnection) : base(UserConnection)
        {
            this.UserConnection = UserConnection;
        }

        protected override Guid GetIntegrationServiceId()
        {
            return CrmConstants.TrcIntegrationServices.ContractsList;
        }

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        protected override ContractsListServiceResponseModel ProcessBusinessLogic(ContractsListServiceRequestModel requestModel, ContractsListServiceResponseModel response)
        {
            Entity contact = null;

            try
            {
                try
                {
                    contact = IntegrationServiceHelper.GetEntityByField(this.UserConnection, "Contact", "TrcProfitbaseLKId", requestModel.clientId);
                }
                catch (Exception ex)
                {

                }

                if (contact == null)
                {
                    if (string.IsNullOrEmpty(requestModel.phone))
                    {
                        response.Result = false;
                        response.Code = 500;
                        response.ReasonPhrase = "В запросе отсутствует номер телефона";
                        return response;
                    }

                    var reversedPhone = IntegrationServiceHelper.GetReversedPhone(requestModel.phone);

                    var esq = new EntitySchemaQuery(this.UserConnection.EntitySchemaManager, "ContactCommunication");

                    esq.AddColumn("Contact");

                    esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "SearchNumber", reversedPhone));

                    var entities = esq.GetEntityCollection(this.UserConnection);

                    if (entities.Count < 1)
                    {
                        response.Result = false;
                        response.Code = 500;
                        response.ReasonPhrase = $"Контакт ни с id {requestModel.clientId}, ни с номером {requestModel.phone} не найден";
                        return response;
                    }
                    else if (entities.Count > 1)
                    {
                        response.Result = false;
                        response.Code = 500;
                        response.ReasonPhrase = $"Найдено более одного контакта с номером {requestModel.phone}";
                        return response;
                    }
                    else
                    {
                        contact = entities.First();

                        contact.SetColumnValue("TrcProfitbaseLKId", requestModel.clientId);
                        contact.SetColumnValue("TrcPersonalAccount", true);

                        contact.Save(false);
                    }
                }

                var esqContracts = new EntitySchemaQuery(this.UserConnection.EntitySchemaManager, "Contract");

                esqContracts.AddAllSchemaColumns();
                esqContracts.AddColumn("State.Name");

                esqContracts.Filters.Add(esqContracts.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcContact", contact.PrimaryColumnValue));
                esqContracts.Filters.Add(esqContracts.CreateFilterWithParameters(FilterComparisonType.NotEqual, "State",    Guid.Parse("BB08517B-C579-4831-975A-E9E870CCDCEB")));
                esqContracts.Filters.Add(esqContracts.CreateFilterWithParameters(FilterComparisonType.NotEqual, "TrcType",  Guid.Parse("C34EB927-F8C0-4D31-8686-12BB423BB42E")));
                esqContracts.Filters.Add(esqContracts.CreateIsNotNullFilter("TrcOpportunity"));

                var contracts = esqContracts.GetEntityCollection(this.UserConnection);

                var contractsData = new List<ContractData>();

                foreach (var item in contracts)
                {
                    contractsData.Add(new ContractData()
                    {
                        contractId = item.GetTypedColumnValue<string>("Id"),
                        contractNum = item.GetTypedColumnValue<string>("Number"),
                        contractDate = item.GetTypedColumnValue<DateTime>("TrcContractDate").ToString("dd-MM-yyyy"),
                        stateContract = new KeyValueData()
                        {
                            name = item.GetTypedColumnValue<string>("State_Name"),
                            value = "#87CEFA"
                        }
                    });
                }

                response.ContractData = contractsData.ToArray();
            }
            catch (Exception ex)
            {
                response.Code = 500;
                response.Result = false;
                response.ReasonPhrase = ex.Message;
            }

            return response;
        }

        protected override void InitRequiredFields(List<string> requiredFields)
        {
            requiredFields.Add("clientId");
            requiredFields.Add("phone");
        }

        protected override void CheckRequiredFields(ContractsListServiceRequestModel request, ContractsListServiceResponseModel response)
        {
            base.CheckRequiredFields(request, response);
        }
    }
}
