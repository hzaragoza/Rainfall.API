using Rainfall.Common.CustomException;
using Rainfall.Common.Model.Validation;
using Rainfall.Common.Validation.Interface;
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
            IsSatisfiedByResult validationResult = new GetReadingSpecification().IsSatisfiedBy(param);
            if (!validationResult.success)
            {
                throw new ResponseCustomException(
                    new ResponseCustomException.ResponseCustomParam()
                    {
                        httpStatusCode = System.Net.HttpStatusCode.BadRequest,
                        strMessage = validationResult.message,
                        ysnCreateLog = true
                    });
            }
            #endregion

            return true;
        }
    }
}
