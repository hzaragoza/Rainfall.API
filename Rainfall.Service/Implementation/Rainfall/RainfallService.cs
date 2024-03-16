using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Rainfall.Common.CustomException;
using Rainfall.Common.Extensions;
using Rainfall.Common.Model.Logger;
using Rainfall.Model.Rainfall;
using Rainfall.Model.Rainfall.Response;
using Rainfall.Repository.Interface;
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
        private readonly ILogger<RainfallService> _logger;
        private readonly IRainfallRepository _iRainfallRepository;

        public RainfallService(ILogger<RainfallService> logger, IRainfallRepository iRainfallRepository) 
        {
            _logger = logger;
            _iRainfallRepository = iRainfallRepository;
        }

        public async Task<rainfallReadingResponse> GetReadings(GetReadingsParam param)
        {
            #region validation
            this.ReadingsValidation(param);
            #endregion

            var rainfallResponse = await _iRainfallRepository.GetReadings(param);
            if (rainfallResponse != null)
            {
                if (rainfallResponse.success)
                {
                    var result = JsonConvert.DeserializeObject<rainfallReadingResponse>(rainfallResponse.json);
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
        #endregion
    }
}
