using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace FitMeApp.Common.FileLogging
{
    public static class FileLoggerExtensions
    {
        public static ILoggerFactory AddFile(this ILoggerFactory factory, string filePath)
        {
            factory.AddProvider(new FileLoggerProvider(filePath));
            return factory;
        }
    }
}
