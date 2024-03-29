﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;

namespace RbiIntegration.Service.BaseClasses
{
    /// <summary>
    /// Базовый класс исходящего сервиса
    /// </summary>
    public class BaseOutService
    {
        protected IntegrationServiceParams _serviceParams { get; set; }
        protected UserConnection _userConnection { get; set; }
        public BaseOutService(UserConnection userConnection, IntegrationServiceParams serviceParams)
        {
            this._serviceParams = serviceParams;
            this._userConnection = userConnection;
        }

        public virtual BaseResponse CallService(params Guid[] id)
        {
            var generator = GetRequestGenerator();
            var handler = GetResponseHandler();

            var model = generator.GenerateModel(id);

            return this.CallService<BaseResponse>(model, handler);
        }

        /// <summary>
        /// Получить генератор запроса
        /// </summary>
        /// <returns></returns>
        protected virtual BaseRequestGenerator GetRequestGenerator(params Guid[] id)
        {
            return new BaseRequestGenerator(this._userConnection, this._serviceParams);
        }

        /// <summary>
        /// Получить обработчик ответа
        /// </summary>
        /// <returns></returns>
        protected virtual BaseResponseHandler GetResponseHandler()
        {
            return new BaseResponseHandler(this._userConnection, this._serviceParams);
        }

        /// <summary>
        /// Вызов сервиса
        /// </summary>
        /// <returns></returns>
        protected virtual T CallService<T>(BaseModel model, BaseResponseHandler handler)
            where T : BaseResponse
        {

            DateTime requestInitDate = DateTime.Now;

            T resultObject = (T)Activator.CreateInstance(typeof(T));

            string requestStr = string.Empty;
            string responseStr = string.Empty;

            var uid = string.Empty;
            var title = string.Empty;

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(this._serviceParams.Url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                httpWebRequest.Credentials = new NetworkCredential(this._serviceParams.Login, this._serviceParams.Password);

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    requestStr = JsonConvert.SerializeObject(model);

                    streamWriter.Write(requestStr);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    responseStr = streamReader.ReadToEnd();
                }

                if(File.Exists(@"C:\123\123.txt"))
                {
                    responseStr = File.ReadAllText(@"C:\123\123.txt");
                }
                
                resultObject = JsonConvert.DeserializeObject<T>(responseStr);

                handler.Handle(resultObject);
            }
            catch (WebException ex)
            {
                resultObject.Code = (int)ex.Status;
                resultObject.Exception = ex.ToString();
            }
            catch (Exception ex)
            {

                if (!ex.Message.Contains("Success")) {
                    resultObject.Exception = ex.ToString();
                    resultObject.StackTrace = ex.StackTrace;
                }
                else
                {
                    resultObject.Exception = ex.Message;
                }
            }
            finally
            {
                responseStr = JsonConvert.SerializeObject(resultObject);

                IntegrationServiceHelper.Log(this._userConnection, this._serviceParams, requestInitDate, title, uid, resultObject == null ? string.Empty : resultObject.Exception, requestStr, responseStr);
            }

            return resultObject;
        }
    }
}
