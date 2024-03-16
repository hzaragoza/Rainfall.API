using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Rainfall.Common.Model.Response
{
    public class httpResponse
    {
        public bool success { get; set; }
        public HttpStatusCode statusCode { get; set; }
        public string json { get; set; }
    }
}
