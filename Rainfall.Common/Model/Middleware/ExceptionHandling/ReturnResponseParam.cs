using Microsoft.AspNetCore.Http;
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
        public string strTransactionID { get; set; } = null;
        public string strMessage { get; set; } = null;
        public HttpContext context { get; set; }
        public HttpStatusCode ResponseStatusCode { get; set; } = HttpStatusCode.BadRequest;
    }
}
