using RbiIntegration.Service.BaseClasses;
using RbiIntegration.Service.Profitbase.Out.Enrichment.Model.Response;
using RbiIntegration.Service.Profitbase.Out.Enrichment.Generator;
using RbiIntegration.Service.Profitbase.Out.Enrichment.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Terrasoft.Core;
using Terrasoft.Core.Entities;
using Terrasoft.Nui.ServiceModel.DataContract;
using Terrasoft.Web.Common;
using BaseResponse = RbiIntegration.Service.BaseClasses.BaseResponse;
using RbiIntegration.Service.Profitbase.Out.AuthToken.Model.Response;

namespace RbiIntegration.Service.Profitbase.Out.EnrichmentService
{
    /// <summary>
    /// Получение обращений
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class EnrichmentService : BaseOutService
    {
        public EnrichmentService(UserConnection userConnection, IntegrationServiceParams serviceParams)
           : base(userConnection, serviceParams)
        {
        }

        protected override BaseRequestGenerator GetRequestGenerator(params string[] id)
        {
            return new EnrichmentServiceRequestGenerator(this._userConnection, this._serviceParams);
        }

        protected override BaseResponseHandler GetResponseHandler()
        {
            return new EnrichmentServiceResponseHandler(this._userConnection, this._serviceParams);
        }

        public override BaseResponse CallService(params string[] id)
        {
            var wrapper = new ServiceWrapper(this._userConnection, "AuthToken");
            var authRes = wrapper.SendRequest();

            this._serviceParams.Token = (authRes as AuthTokenServiceResponseModel).token;

            var generator = GetRequestGenerator();
            var handler = GetResponseHandler();

            var model = generator.GenerateModel(id);

            return this.CallService<EnrichmentServiceResponseModel>(model, handler);
        }
    }
}
