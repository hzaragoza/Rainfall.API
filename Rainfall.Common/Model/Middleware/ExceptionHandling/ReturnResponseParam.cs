using Microsoft.AspNetCore.Http;
using Rainfall.Common.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Rainfall.Common.Model.Middleware.ExceptionHandling
{
    public class ReturnResponseParam
    {
        public error error { get; set; }
        public HttpContext context { get; set; }
        public HttpStatusCode ResponseStatusCode { get; set; } = HttpStatusCode.BadRequest;
    }
}
