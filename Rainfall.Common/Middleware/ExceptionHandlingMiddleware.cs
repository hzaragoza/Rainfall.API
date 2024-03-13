using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Rainfall.Common.CustomException;
using Rainfall.Common.Model.Logger;
using Rainfall.Common.Model.Middleware.ExceptionHandling;
using Rainfall.Common.Model.Response;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Rainfall.Common.Middleware
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) 
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex) 
            {
                string? traceId = Activity.Current?.Id ?? context.TraceIdentifier;

                ExceptionDetails exceptionDetail = this.GetExceptionDetails(traceId, ex);
                if (exceptionDetail.ysnCreateLog)
                {
                    var log = new ApiLog()
                    {
                        strTransactionId = traceId,
                        strEndpoint = this.GetEndpoint(context),
                        strMessage = exceptionDetail.error.message
                    };

                    _logger.Log(LogLevel.Critical, $"{JsonConvert.SerializeObject(log)},");
                }

                await this.ReturnResponse(
                    new ReturnResponseParam()
                    { 
                        error = exceptionDetail.error,
                        context = context,
                        ResponseStatusCode = exceptionDetail.ResponseStatusCode
                    });
            }

            return;
        }


        #region private
        private ExceptionDetails GetExceptionDetails(string traceId, Exception ex)
        {
            var result = new ExceptionDetails();

            switch (ex)
            {
                case ResponseCustomException:
                    var customEx = ex as ResponseCustomException;

                    result.ysnCreateLog = customEx.ysnCreateLog;
                    result.error = customEx.error;
                    result.ResponseStatusCode = customEx.httpStatusCode;
                    break;

                case Exception:
                    result.ysnCreateLog = true;
                    result.error = new Model.Response.error()
                    { 
                        message = $"TraceId:{traceId}. {ex.Message}"
                    };
                    result.strExceptionMessage = ex.InnerException?.Message;
                    result.ResponseStatusCode = HttpStatusCode.InternalServerError;
                    break;
            }

            return result;
        }
        private Task ReturnResponse(ReturnResponseParam param)
        {
            var errorMessage =
                    JsonConvert.SerializeObject(
                        param.error);

            param.context.Response.StatusCode = (int)param.ResponseStatusCode;
            param.context.Response.ContentType = "application/json";

            return param.context.Response.WriteAsync(errorMessage);
        }
        private string GetEndpoint(HttpContext context)
        {
            var request = context.Request;

            var requestPath = request.Path.HasValue ? context.Request.Path.Value : null;
            var queryString = request.QueryString.HasValue ? request.QueryString.Value : null;

            string domain = $"{request.Scheme}://{request.Host.Value}";

            string strEndpoint = domain + requestPath + queryString;
            return strEndpoint;
        }
        #endregion
    }
}
