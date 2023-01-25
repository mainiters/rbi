using RbiIntegration.Service.BaseClasses;
using RbiIntegration.Service.Profitbase.In.ContractsPaymentScheduleService.Model.Request;
using RbiIntegration.Service.Profitbase.In.ContractsPaymentScheduleService.Model.Response;
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

namespace RbiIntegration.Service.Profitbase.In.ContractsPaymentScheduleService
{
    /// <summary>
    /// Запрос на получение графика платежей по договору
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class ContractsPaymentScheduleService : BaseRbiService<ContractsPaymentScheduleServiceRequestModel, ContractsPaymentScheduleServiceResponseModel>
    {
        public ContractsPaymentScheduleService(UserConnection UserConnection) : base(UserConnection)
        {
            this.UserConnection = UserConnection;
        }

        protected override Guid GetIntegrationServiceId(ContractsPaymentScheduleServiceRequestModel requestModel)
        {
            return CrmConstants.TrcIntegrationServices.ContractsPaymentSchedule;
        }

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        protected override ContractsPaymentScheduleServiceResponseModel ProcessBusinessLogic(ContractsPaymentScheduleServiceRequestModel requestModel, ContractsPaymentScheduleServiceResponseModel response)
        {
            Entity contract = null;
            Entity request = null;

            try
            {
                contract = IntegrationServiceHelper.GetEntityByField(this.UserConnection, "Contract", "Id", requestModel.contractId);

                var esq = new EntitySchemaQuery(this.UserConnection.EntitySchemaManager, "Opportunity");

                esq.AddAllSchemaColumns();

                esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcOpportunity", contract.GetTypedColumnValue<Guid>("TrcOpportunityId")));
                esq.Filters.Add(esq.CreateIsNotNullFilter("TrcFinalFreeCalculation"));
                esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcContractSigned", true));

                var opportunity = esq.GetEntityCollection(this.UserConnection).FirstOrDefault();

                try
                {
                    var product = IntegrationServiceHelper.GetEntityByField(this.UserConnection, "Product", "Id", opportunity.GetTypedColumnValue<Guid>("TrcObjectId"));

                    int objectId;

                    if (int.TryParse(product.GetTypedColumnValue<string>("TrcObjectId"), out objectId))
                    {
                        response.objectId = objectId;
                    }
                }
                catch
                {
                    response.Result = false;
                    response.ReasonPhrase = "График платежей не найден";
                }

                response.contractNum = contract.GetTypedColumnValue<string>("Number");
                response.amountContract = contract.GetTypedColumnValue<float>("TrcContractPrice");

                esq = new EntitySchemaQuery(this.UserConnection.EntitySchemaManager, "TrcPaymentScheduleInFreeCalculation");

                esq.AddAllSchemaColumns();

                esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "TrcTrcFreeCalculation", opportunity.GetTypedColumnValue<Guid>("TrcFinalFreeCalculationId")));

                var paymentScheduleInFreeCalculations = esq.GetEntityCollection(this.UserConnection);

                if (paymentScheduleInFreeCalculations.Count > 0)
                {
                    response.dateUntil = paymentScheduleInFreeCalculations.OrderByDescending(e => e.GetTypedColumnValue<DateTime>("TrcDatePayment")).First().GetTypedColumnValue<DateTime>("TrcDatePayment").ToString("dd.MM.yyyy");
                    response.balanceContract = response.amountContract - paymentScheduleInFreeCalculations
                        .Where(e => e.GetTypedColumnValue<bool>("TrcPaid"))
                        .Sum(e => e.GetTypedColumnValue<float>("TrcAmountPayment"));

                    response.plannedPayments = new List<PlannedPayments>();
                    response.paidPayments = new List<PaidPayments>();

                    foreach (var item in paymentScheduleInFreeCalculations.Where(e => !e.GetTypedColumnValue<bool>("TrcPaid")))
                    {
                        response.plannedPayments.Add(new PlannedPayments()
                        {
                            datePayment = item.GetTypedColumnValue<DateTime>("TrcDatePayment").ToString("dd-MM-yyyy"),
                            amountPayment = item.GetTypedColumnValue<float>("TrcAmountPayment"),
                            latePayment = item.GetTypedColumnValue<DateTime>("TrcDatePayment").Date >= DateTime.Now.Date ? 0 : 1
                        });
                    }

                    foreach (var item in paymentScheduleInFreeCalculations.Where(e => e.GetTypedColumnValue<bool>("TrcPaid")))
                    {
                        response.paidPayments.Add(new PaidPayments()
                        {
                            datePayment = item.GetTypedColumnValue<DateTime>("TrcDatePayment").ToString("dd-MM-yyyy"),
                            amountPayment = item.GetTypedColumnValue<float>("TrcAmountPayment")
                        });
                    }
                }
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
            requiredFields.Add("contractId");
        }

        protected override void CheckRequiredFields(ContractsPaymentScheduleServiceRequestModel request, ContractsPaymentScheduleServiceResponseModel response)
        {
            base.CheckRequiredFields(request, response);
        }
    }
}
