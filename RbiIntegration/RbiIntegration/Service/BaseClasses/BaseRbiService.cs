using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Web.Common;
using static RbiIntegration.Service.BaseClasses.CrmConstants;

namespace RbiIntegration.Service.BaseClasses
{
    /// <summary>
    /// Базовый класс, реализующий общую логику сервисов РБИ
    /// </summary>
    public abstract class BaseRbiService<Rq, Rs> : BaseService
        where Rq : BaseModel
        where Rs : BaseResponse
    {
        /// <summary>
        /// Список обязательных тегов (полей модели) в тексте запроса
        /// </summary>
        protected List<string> RequiredFields { get; private set; }

        public BaseRbiService()
        {
            this.RequiredFields = new List<string>();
        }

        /// <summary>
        /// Обработка входящего запроса
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual Rs ProcessRequest(Rq requestModel)
        {
            DateTime requestInitDate = DateTime.Now;

            var request = IntegrationServiceHelper.ToJson(requestModel);

            var response = (Rs)Activator.CreateInstance(typeof(Rs));

            var uid = string.Empty;
            var title = string.Empty;

            try
            {
                this.InitRequiredFields(this.RequiredFields);
                this.CheckRequiredFields(requestModel, response);
                
                this.ProcessBusinessLogic(requestModel, response);
            }
            catch (Exception ex)
            {
                response.Code = 500;
                response.Message = "ERROR";
                response.Exception = ex.Message;
                response.StackTrace = ex.StackTrace;
                response.Result = false;
            }
            finally
            {
                IntegrationServiceHelper.Log(UserConnection, new IntegrationServiceParams() { Id = TrcIntegrationServices.CreateClientObjectRelation }, requestInitDate, title, uid, response.Exception, request, IntegrationServiceHelper.ToJson(response), response != null ? response.Code : 0);
            }

            return response;
        }

        /// <summary>
        /// Бизнеовая логика обработки запроса
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        protected abstract Rs ProcessBusinessLogic(Rq requestModel, Rs response);

        /// <summary>
        /// Инициализация списка обязательных полей
        /// </summary>
        protected abstract void InitRequiredFields(List<string> requiredFields);

        /// <summary>
        /// Порверяет заполненность обязательных полей в запросе
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual void CheckRequiredFields(Rq request, Rs response)
        {
            foreach (var item in this.RequiredFields)
            {
                var propValue = request.GetType().GetProperty(item).GetValue(request, null);

                if (propValue == null || string.IsNullOrEmpty(propValue.ToString()))
                {
                    response.ReasonPhrase = $"Обязательное поле {item} не заполнено";
                    response.Code = 304001;
                    return;
                }
            }
        }
    }
}
