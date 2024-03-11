using Rainfall.Common.CustomException;
using Rainfall.Model.Rainfall;
using Rainfall.Service.Implementation.Rainfall.Validation;
using Rainfall.Service.Interface.Rainfall;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainfall.Service.Implementation.Rainfall
{
    public class RainfallService : IRainfallService
    {
        public async Task<bool> GetReadings(GetReadingsParam param)
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

            return true;
        }
    }
}
