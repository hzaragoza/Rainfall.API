using Newtonsoft.Json;
using Rainfall.Common.CustomException;
using Rainfall.Common.Extensions;
using Rainfall.Model.Rainfall;
using Rainfall.Model.Rainfall.Response;
using Rainfall.Service.Implementation.Rainfall.Validation;
using Rainfall.Service.Interface.Rainfall;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Rainfall.Service.Implementation.Rainfall
{
    public class RainfallService : IRainfallService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RainfallService(IHttpClientFactory httpClientFactory) 
        {
            _httpClientFactory = httpClientFactory;
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

            return await client.GetAsync(strEndpoint);
        }
        #endregion
    }
}
