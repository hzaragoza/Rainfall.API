using Rainfall.Common.Model.Validation;
using Rainfall.Common.Validation.Specification.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainfall.Common.Validation.Specification.Implementation
{
    public class ExpressionSpecification<T> : BaseSpecification<T>
    {
        private readonly Func<T, bool> _expression;

        public ExpressionSpecification(Func<T, bool> expression)
        {
            _expression = expression;
        }

        public override bool IsSatisfiedBy(T obj)
        {
            return _expression(obj);
        }
    }
}
