using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Rainfall.Common.CustomException;
using Rainfall.Common.Model.Middleware.ExceptionHandling;
using System;
using System.Collections.Generic;
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
            string strTransactionID = Guid.NewGuid().ToString();

            try
            {
                await next(context);
            }
            catch (Exception ex) 
            { 
                ExceptionDetails exceptionDetail = this.GetExceptionDetails(ex);
                if (exceptionDetail.ysnCreateLog)
                {
                    _logger.Log(LogLevel.Critical, exceptionDetail.strMessage);
                }

                await this.ReturnResponse(
                    new ReturnResponseParam()
                    { 
                        strTransactionID = strTransactionID,
                        strMessage = exceptionDetail.strMessage,
                        context = context,
                        ResponseStatusCode = exceptionDetail.ResponseStatusCode
                    });
            }

            return;
        }


        #region private
        private ExceptionDetails GetExceptionDetails(Exception ex)
        {
            var result = new ExceptionDetails();

            switch (ex)
            {
                case ResponseCustomException:
                    var customEx = ex as ResponseCustomException;

                    result.ysnCreateLog = customEx.ysnCreateLog;
                    result.strMessage = customEx.strMessage;
                    result.ResponseStatusCode = customEx.httpStatusCode;
                    break;

                case Exception:
                    result.ysnCreateLog = true;
                    result.strMessage = ex.Message;
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
                        new ErrorResponse()
                        {
                            transactionId = param.strTransactionID,
                            message = param.strMessage
                        });

            param.context.Response.StatusCode = (int)param.ResponseStatusCode;
            param.context.Response.ContentType = "application/json";

            return param.context.Response.WriteAsync(errorMessage);
        }
        #endregion
    }
}
