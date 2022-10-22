using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace FitMeApp.Common.FileLogging
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private string filePath;
        public FileLoggerProvider(string path)
        {
            filePath = path;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(filePath);
        }

        public void Dispose()
        {
           
        }
    }
}
