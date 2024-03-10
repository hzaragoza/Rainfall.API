using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainfall.Common.Model.Validation
{
    public class IsSatisfiedByResult
    {
        public bool success { get; set; } = false;
        public string message { get; set; } = null;
    }
}
