using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace FitMeApp.Common.FileLogging
{
    public static class FileLoggerExtensions
    {
        public static void AddFile(this ILoggerFactory factory, string filePath)
        {
            factory.AddProvider(new FileLoggerProvider(filePath));
        }
    }
}
