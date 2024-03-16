using Rainfall.Common.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Rainfall.Common.Model.Middleware.ExceptionHandling
{
    public class ExceptionDetails
    {
        public bool ysnCreateLog { get; set; } = false;
        public string strExceptionMessage { get; set; } = null;
        public HttpStatusCode ResponseStatusCode { get; set; } = HttpStatusCode.BadRequest;
        public error error { get; set; }
    }
}
