using Newtonsoft.Json;
using Rainfall.Common.CustomException;
using Rainfall.Common.Extensions;
using Rainfall.Model.Rainfall;
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

        public async Task<StationReadingResult> GetReadings(GetReadingsParam param)
        {
            #region validation
            var stationIdSpec = new StationIdSpecification();
            bool isStationSpecSatidfied = stationIdSpec.IsSatisfiedBy(param);
            if (!isStationSpecSatidfied)
            {
                throw new ResponseCustomException(
                    new ResponseCustomException.ResponseCustomParam()
                    {
                        httpStatusCode = System.Net.HttpStatusCode.BadRequest,
                        strMessage = "Please provide stationId.",
                        ysnCreateLog = true
                    });
            }

            var countSpec = new CountSpecification();
            bool isCountSpecSatidfied = countSpec.IsSatisfiedBy(param);
            if (!isCountSpecSatidfied)
            {
                throw new ResponseCustomException(
                    new ResponseCustomException.ResponseCustomParam()
                    {
                        httpStatusCode = System.Net.HttpStatusCode.BadRequest,
                        strMessage = "Provided count is not in range.",
                        ysnCreateLog = true
                    });
            }
            #endregion

            var rainfallResponse = await this.httpClientGet($"/flood-monitoring/id/stations/{param.stationId}/readings?_sorted&_limit={param.count}");
            string jsonDeliveryStatusResponse = await rainfallResponse.Content.ReadAsStringAsync();
            if (rainfallResponse.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<StationReadingResult>(jsonDeliveryStatusResponse);
                if (result != null)
                {
                    if (!result.items.HasRecord())
                    {
                        throw new ResponseCustomException(
                            new ResponseCustomException.ResponseCustomParam()
                            {
                                httpStatusCode = System.Net.HttpStatusCode.NotFound,
                                strMessage = "No readings found for the specified stationId",
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
                    strMessage = "Internal server error",
                    ysnCreateLog = true
                });
        }

        #region private
        private async Task<HttpResponseMessage> httpClientGet(string strEndpoint)
        {
            var client = _httpClientFactory.CreateClient("httpclient-rainfall");

            return await client.GetAsync(strEndpoint);
        }
        #endregion
    }
}
