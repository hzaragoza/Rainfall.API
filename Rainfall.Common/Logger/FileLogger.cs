using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainfall.Common.Logger
{
    public class FileLogger : ILogger
    {
        protected readonly FileLoggerProvider _fileLoggerProvider;
        private static readonly object locker = new object();

        public FileLogger([NotNull] FileLoggerProvider fileLoggerProvider)
        {
            _fileLoggerProvider = fileLoggerProvider;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            var dtmDateNow = DateTime.Now;

            string stateMessage = formatter(state, exception);

            #region Check and create sub folder
            string strFullWithSubFolder = Path.Combine(_fileLoggerProvider.Options.strFolderPath, logLevel.ToString());
            if (!Directory.Exists(strFullWithSubFolder))
            {
                Directory.CreateDirectory(strFullWithSubFolder);
            }

            var strFullFilePath = Path.Combine(strFullWithSubFolder, $"{dtmDateNow.ToString("yyyyMMdd")}.txt");
            #endregion

            lock (locker)
            {
                try
                {
                    using (FileStream fs = new FileStream(strFullFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    using (var streamWriter = new StreamWriter(fs))
                    {
                        streamWriter.WriteLine(stateMessage);
                    }
                }
                catch
                {
                }
            }
        }
    }
}
