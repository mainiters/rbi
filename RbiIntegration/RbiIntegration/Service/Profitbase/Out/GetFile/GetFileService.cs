using RbiIntegration.Service.BaseClasses;
using RbiIntegration.Service.Profitbase.Out.GetFile.Model.Response;
using RbiIntegration.Service.Profitbase.Out.GetFile.Generator;
using RbiIntegration.Service.Profitbase.Out.GetFile.Handler;
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

namespace RbiIntegration.Service.Profitbase.Out.GetFile
{
    /// <summary>
    /// Получение файлов
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class GetFileService : BaseOutService
    {
        public GetFileService(UserConnection userConnection, IntegrationServiceParams serviceParams)
           : base(userConnection, serviceParams)
        {
        }

        protected override BaseRequestGenerator GetRequestGenerator(params string[] id)
        {
            return new GetFileServiceRequestGenerator(this._userConnection, this._serviceParams);
        }

        protected override BaseResponseHandler GetResponseHandler()
        {
            return new GetFileServiceResponseHandler(this._userConnection, this._serviceParams);
        }

        public override BaseResponse CallService(params string[] id)
        {
            var wrapper = new ServiceWrapper(this._userConnection, "AuthToken");
            var authRes = wrapper.SendRequest();

            this._serviceParams.Token = (authRes as AuthTokenServiceResponseModel).token;

            var generator = GetRequestGenerator();
            var handler = GetResponseHandler();

            var model = generator.GenerateModel(id);

            return this.CallService<GetFileServiceResponseModel>(model, handler);
        }
    }
}
