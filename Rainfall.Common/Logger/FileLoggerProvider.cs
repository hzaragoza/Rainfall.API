using Microsoft.Extensions.Logging;
using Rainfall.Common.Model.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainfall.Common.Logger
{
    public class FileLoggerProvider : ILoggerProvider
    {
        // protected readonly IHttpContextAccessor _iHttpContextAccessor;
        public readonly FileLoggerOptions Options;

        public FileLoggerProvider(string strContentRootPath)
        {
            //_iHttpContextAccessor = iHttpContextAccessor;
            #region Check and create log folder
            string strLogFolder = CreateLogFolder(strContentRootPath);
            bool ysnCreateLogLevelFolders = CreateLogLevelFolders(strLogFolder);
            #endregion

            Options = new FileLoggerOptions()
            {
                strFolderPath = strLogFolder,
                strFilePath = string.Empty
            };
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(this);
        }

        public void Dispose()
        {
        }

        public string CreateLogFolder(string strContentRootPath)
        {
            string strLogFolder = Path.Combine(strContentRootPath, "Logs");
            if (!Directory.Exists(strLogFolder))
            {
                Directory.CreateDirectory(strLogFolder);
            }

            return strLogFolder;
        }

        public bool CreateLogLevelFolders(string strLogFolder)
        {
            var logLevelList = System.Enum.GetValues(typeof(LogLevel)).Cast<LogLevel>().ToList();

            if (logLevelList == null
                || !logLevelList.Any())
            {
                return false;
            }

            foreach (var level in logLevelList)
            {
                string strFullWithSubFolder = Path.Combine(strLogFolder, level.ToString());
                if (!Directory.Exists(strFullWithSubFolder))
                {
                    Directory.CreateDirectory(strFullWithSubFolder);
                }
            }

            return true;
        }
    }
}
