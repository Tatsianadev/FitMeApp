using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FitMeApp.Common
{
    public static class DefaultSettingsStorage
    {
        public static  string ApplicationName { get; set; }
        public static string AdminEmail { get; set; }
        public static string AdminPassword { get; set; }
        public static string SenderEmail { get; set; }
        public static string ReceiverEmail { get; set; }
        public static string CronForSendEmailJob { get; set; }
        public static string SendEmailJobIfDays { get; set; }


    }
}
