using Rainfall.Common.Model.Validation;
using Rainfall.Common.Validation.Specification.Implementation;
using Rainfall.Common.Validation.Specification.Interface;
using Rainfall.Model.Rainfall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainfall.Service.Implementation.Rainfall.Validation
{
    public class CountSpecification : BaseSpecification<GetReadingsParam>
    {
        public override bool IsSatisfiedBy(GetReadingsParam reading)
        {
            return reading.count >= 1 && reading.count <= 100;
        }
    }
}
