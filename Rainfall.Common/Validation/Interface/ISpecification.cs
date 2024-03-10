using Rainfall.Common.Model.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainfall.Common.Validation.Interface
{
    public interface ISpecification<T>
    {
        IsSatisfiedByResult IsSatisfiedBy(T item);
    }
}
