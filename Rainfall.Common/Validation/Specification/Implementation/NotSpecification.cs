using Rainfall.Common.Model.Validation;
using Rainfall.Common.Validation.Specification.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainfall.Common.Validation.Specification.Implementation
{
    public class NotSpecification<T> : BaseSpecification<T>
    {
        private readonly ISpecification<T> _otherSpecification;

        public NotSpecification(ISpecification<T> otherSpecification)
        {
            _otherSpecification = otherSpecification;
        }

        public override bool IsSatisfiedBy(T obj)
        { 
            return !_otherSpecification.IsSatisfiedBy(obj);
        }
    }
}
