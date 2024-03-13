using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Rainfall.Common.CustomException;
using Rainfall.Common.Extensions;
using Rainfall.Common.Model.Logger;
using Rainfall.Model.Rainfall;
using Rainfall.Model.Rainfall.Response;
using Rainfall.Service.Implementation.Rainfall.Validation;
using Rainfall.Service.Interface.Rainfall;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Rainfall.Service.Implementation.Rainfall
{
    public class RainfallService : IRainfallService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<RainfallService> _logger;
        private const int intRetryCount = 2;

        public RainfallService(IHttpClientFactory httpClientFactory, ILogger<RainfallService> logger) 
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<rainfallReadingResponse> GetReadings(GetReadingsParam param)
        {
            #region validation
            this.ReadingsValidation(param);
            #endregion

            var rainfallResponse = await this.httpClientGet($"/flood-monitoring/id/stations/{param.stationId}/readings?_sorted&_limit={param.count}");
            string jsonDeliveryStatusResponse = await rainfallResponse.Content.ReadAsStringAsync();
            if (rainfallResponse.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<rainfallReadingResponse>(jsonDeliveryStatusResponse);
                if (result != null)
                {
                    if (!result.readings.HasRecord())
                    {
                        throw new ResponseCustomException(
                            new ResponseCustomException.ResponseCustomParam()
                            {
                                httpStatusCode = System.Net.HttpStatusCode.NotFound,
                                error = new Common.Model.Response.error()
                                {
                                    message = "No readings found for the specified stationId"
                                },
                                ysnCreateLog = true
                            });
                    }

                    return result;
                }
            }

            throw new ResponseCustomException(
                new ResponseCustomException.ResponseCustomParam()
                {
                    httpStatusCode = System.Net.HttpStatusCode.InternalServerError,
                    error = new Common.Model.Response.error()
                    {
                        message = "Something went wrong"
                    },
                    ysnCreateLog = true
                });
        }

        #region private
        private void ReadingsValidation(GetReadingsParam param)
        {
            var error = new Common.Model.Response.error()
            {
                message = "Invalid request"
            };

            var stationIdSpec = new StationIdSpecification();
            bool isStationSpecSatidfied = stationIdSpec.IsSatisfiedBy(param);
            if (!isStationSpecSatidfied)
            {
                error.detail.Add(
                    new Common.Model.Response.errorDetail()
                    {
                        propertyName = "station",
                        message = "Please provide stationId."
                    });
            }

            var countSpec = new CountSpecification();
            bool isCountSpecSatidfied = countSpec.IsSatisfiedBy(param);
            if (!isCountSpecSatidfied)
            {
                error.detail.Add(
                    new Common.Model.Response.errorDetail()
                    {
                        propertyName = "count",
                        message = "Provided count is not in range."
                    });
            }

            if (error.detail.HasRecord())
            {
                throw new ResponseCustomException(
                    new ResponseCustomException.ResponseCustomParam()
                    {
                        httpStatusCode = System.Net.HttpStatusCode.BadRequest,
                        error = error,
                        ysnCreateLog = true
                    });
            }
        }
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
