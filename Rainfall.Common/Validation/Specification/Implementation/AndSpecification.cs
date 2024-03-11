using Rainfall.Common.Model.Validation;
using Rainfall.Common.Validation.Specification.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainfall.Common.Validation.Specification.Implementation
{
    public class AndSpecification<T> : BaseSpecification<T>
    {
        private readonly ISpecification<T> _leftSpecification;
        private readonly ISpecification<T> _rightSpecification;

        public AndSpecification(ISpecification<T> leftSpecification, ISpecification<T> rightSpecification)
        {
            _leftSpecification = leftSpecification;
            _rightSpecification = rightSpecification;
        }

        public override bool IsSatisfiedBy(T obj)
        { 
            return _leftSpecification.IsSatisfiedBy(obj) || _rightSpecification.IsSatisfiedBy(obj);
        }
    }
}
