using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rainfall.Common.Model.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainfall.Common.Logger
{
    public static class FileLoggerExtensions
    {
        public static ILoggingBuilder AddFileLogger(
            this ILoggingBuilder builder, 
            Action<FileLoggerOptions> configure)
        {
            builder.Services.AddSingleton<ILoggerProvider, FileLoggerProvider>();
            builder.Services.Configure(configure);
            return builder;
        }
    }
}
