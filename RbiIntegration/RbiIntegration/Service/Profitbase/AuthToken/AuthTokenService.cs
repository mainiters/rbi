using RbiIntegration.Service.BaseClasses;
using RbiIntegration.Service.In.Profitbase.AuthToken.Model.Response;
using RbiIntegration.Service.In.Profitbase.Enrichment.Model.Response;
using RbiIntegration.Service.Profitbase.AuthToken.Generator;
using RbiIntegration.Service.Profitbase.AuthToken.Handler;
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

namespace RbiIntegration.Service.In.Profitbase.AuthToken
{
    /// <summary>
    /// Получение обращений
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class AuthTokenService : BaseOutService
    {
        public AuthTokenService(UserConnection userConnection, IntegrationServiceParams serviceParams)
           : base(userConnection, serviceParams)
        {
        }

        protected override BaseRequestGenerator GetRequestGenerator(params Guid[] id)
        {
            return new AuthTokenServiceRequestGenerator(this._userConnection, this._serviceParams);
        }

        protected override BaseResponseHandler GetResponseHandler()
        {
            return new AuthTokenServiceResponseHandler(this._userConnection, this._serviceParams);
        }

        public override BaseResponse CallService(params Guid[] id)
        {
            var generator = GetRequestGenerator();
            var handler = GetResponseHandler();

            var model = generator.GenerateModel(id);

            return this.CallService<AuthTokenServiceResponseModel>(model, handler);
        }
    }
}
