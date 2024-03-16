using Rainfall.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Polly;
using Newtonsoft.Json;
using System.Net;
using Rainfall.Common.Model.Logger;
using Rainfall.Common.CustomException;
using Rainfall.Model.Rainfall.Response;
using Rainfall.Model.Rainfall;
using Rainfall.Common.Model.Response;

namespace Rainfall.Repository.Implementation
{
    public class RainfallRepository : IRainfallRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<RainfallRepository> _logger;
        private const int intRetryCount = 2;

        public RainfallRepository(IHttpClientFactory httpClientFactory, ILogger<RainfallRepository> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<httpResponse> GetReadings(GetReadingsParam param)
        {
            var rainfallResponse = await this.httpClientGet($"/flood-monitoring/id/stations/{param.stationId}/readings?_sorted&_limit={param.count}");
            string json = await rainfallResponse.Content.ReadAsStringAsync();

            return new httpResponse()
            {
                success = rainfallResponse.IsSuccessStatusCode,
                statusCode = rainfallResponse.StatusCode,
                json = json
            };
        }

        #region private
        private async Task<HttpResponseMessage> httpClientGet(string strEndpoint)
        {
            var client = _httpClientFactory.CreateClient("httpclient-rainfall");

            #region Create Policy
            var policy =
                Policy.Handle<Exception>()
                      .OrResult<HttpResponseMessage>(r =>
                      {
                          const bool proceedToRetry = true;
                          const bool doNotRetry = false;

                          if (!r.IsSuccessStatusCode)
                          {
                              if (r.StatusCode == HttpStatusCode.TooManyRequests)
                              {
                                  return proceedToRetry;
                              }
                          }

                          return doNotRetry;
                      })
                      .WaitAndRetryAsync(
                            intRetryCount,
                            attempt => TimeSpan.FromSeconds(5),
                            (ex, _, retryCount, ctx) =>
                            {
                                #region Get Error message and Status Code
                                string strErrorMessage = (ex != null && ex.Exception != null)
                                                              ? $" Error message: {ex.Exception.Message}"
                                                              : System.String.Empty;
                                string strInnerErrorMessage = (ex != null && ex.Exception != null && ex.Exception.InnerException != null)
                                                                  ? $" Inner Exception message: {ex.Exception.InnerException.Message}"
                                                                  : System.String.Empty;

                                string strStatusCodeInfo = (ex != null && ex.Result != null)
                                                                ? $" Status Code: ({(int)ex.Result.StatusCode}){ex.Result.StatusCode.ToString()}."
                                                                : System.String.Empty;
                                #endregion

                                #region Get Payload and Response
                                var response = ex?.Result?.Content?.ReadAsStringAsync().Result;
                                #endregion

                                #region Create Log
                                _logger.Log(
                                    LogLevel.Critical,
                                    $"{JsonConvert.SerializeObject(new ApiLog()
                                    {
                                        strMessage = $"Retry count: {retryCount}. Waited until 5sec before it execute attempt # {retryCount}.{strStatusCodeInfo}{strErrorMessage}{strInnerErrorMessage}"
                                    })},");
                                #endregion
                            });
            #endregion

            return await policy.ExecuteAsync(() => client.GetAsync(strEndpoint));
        }
        #endregion
    }
}
