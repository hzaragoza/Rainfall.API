using Rainfall.Common.Model.Validation;
using Rainfall.Common.Validation.Interface;
using Rainfall.Model.Rainfall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainfall.Service.Implementation.Rainfall.Validation
{
    public class GetReadingSpecification : ISpecification<GetReadingsParam>
    {
        public IsSatisfiedByResult IsSatisfiedBy(GetReadingsParam item)
        {
            if (String.IsNullOrEmpty(item.stationId))
            {
                return new IsSatisfiedByResult()
                { 
                    success = false,
                    message = "Please provide stationId."
                };
            }
            else if (item.count < 1 || item.count > 100)
            {
                return new IsSatisfiedByResult()
                {
                    success = false,
                    message = "Provided count is not in range."
                };
            }

            return new IsSatisfiedByResult()
            {
                success = true,
                message = null
            };
        }
    }
}
