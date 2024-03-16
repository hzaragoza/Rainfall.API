using Rainfall.Common.Model.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainfall.Common.Validation.Specification.Interface
{
    public interface ISpecification<T>
    {
        ISpecification<T> And(ISpecification<T> specification);
        ISpecification<T> Or(ISpecification<T> specification);
        ISpecification<T> Not(ISpecification<T> specification);

        bool IsSatisfiedBy(T obj);
    }
}
