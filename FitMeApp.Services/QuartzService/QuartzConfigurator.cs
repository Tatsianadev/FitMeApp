using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Microsoft.Extensions.Configuration;
using Quartz;

namespace FitMeApp.Services.QuartzService
{
    public static class QuartzConfigurator
    {
        public static void AddJobAndTrigger<T>(this IServiceCollectionQuartzConfigurator quartz) where T: IJob
        {
            string jobName = typeof(T).Name;
            var cronSchedule = Common.DefaultSettingsStorage.CronForSendEmailJob;

            if (string.IsNullOrEmpty(cronSchedule))
            {
                throw new Exception("No Quartz.NET Cron schedule found for job in configuration.");
            }

            var jobKey = new JobKey(jobName);
            quartz.AddJob<T>(opts => opts.WithIdentity(jobKey));

            quartz.AddTrigger(opts => opts
                .ForJob(jobKey).WithIdentity(jobName + "Trigger").WithCronSchedule(cronSchedule));
        }

    }
}
