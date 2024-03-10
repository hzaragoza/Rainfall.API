using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainfall.Common.Model.Middleware.ExceptionHandling
{
    public class ErrorResponse
    {
        public string transactionId { get; set; }
        public string message { get; set; }
    }
}
