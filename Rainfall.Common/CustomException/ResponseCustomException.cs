using Rainfall.Common.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Rainfall.Common.CustomException
{
    public class ResponseCustomException : Exception
    {
        #region model
        public class ResponseCustomParam
        {
            /// <summary>
            /// The status code to use when returning the response.
            /// </summary>
            public HttpStatusCode httpStatusCode { get; set; } = HttpStatusCode.BadRequest;

            /// <summary>
            /// This boolean is use to tell the API if the custom exception needs to be logged in the API.
            /// </summary>
            public bool ysnCreateLog { get; set; }

            public error error { get; set; }
        }
        #endregion

        public ResponseCustomException(ResponseCustomParam param)
        {
            this.httpStatusCode = param.httpStatusCode;
            this.error = param.error;
            this.ysnCreateLog = param.ysnCreateLog;
        }

        public override string StackTrace
        {
            get { return null; }
        }

        /// <summary>
        /// This boolean is use to tell the API if the custom exception needs to be logged in the API.
        /// </summary>
        public bool ysnCreateLog { get; }

        /// <summary>
        /// The status code to use when returning the response.
        /// </summary>
        public HttpStatusCode httpStatusCode { get; }

        public error error { get; }
    }
}
