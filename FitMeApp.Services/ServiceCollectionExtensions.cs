using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FitMeApp.Repository.EntityFramework;
using FitMeApp.Common;
using FitMeApp.Repository.EntityFramework.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using System.Globalization;
using FitMeApp.Services.QuartzService;
using Microsoft.AspNetCore.Localization;
using Quartz;

namespace FitMeApp.Services
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
        }

        public static void RegisterIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
                
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();            
        }

        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddScoped<IRepository, Repository.EntityFramework.Repository>();
            services.AddScoped<IGymService, GymService>();
            services.AddScoped<ITrainerService, TrainerService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<ITrainingService, TrainingService>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddScoped<IDietService, DietService>();
            services.AddScoped<IProductsService, ProductsService>();
            services.AddScoped<IFileStorage, FileStorage>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IReportService, ReportService>();
            //services.AddScoped<IExcelReport, ExcelReportEpPlus>();
            services.AddScoped<IExcelReport, ExcelReportOpenXml>();
            services.AddScoped<IPdfReport, PdfReportAspose>();
            services.AddScoped<ITextReport, TextReport>();
        }

        public static void RegisterMailService(this IServiceCollection services, string apiKey)
        {
            services.AddSingleton<IEmailService>(emailService => new EmailService(apiKey));
        }

        public static void RegisterSchedulerService(this IServiceCollection services)
        {
            services.AddQuartz(q =>
                {
                    q.UseMicrosoftDependencyInjectionJobFactory();
                    q.AddJobAndTrigger<SendEmailJob>();
                });

            services.AddQuartzHostedService(q =>
                q.WaitForJobsToComplete = true);
        }


        public static void InitializeDefaultSettings(this IServiceCollection services, IConfiguration configuration)
        {
            Common.DefaultSettingsStorage.ApplicationName = configuration.GetSection("Constants")["ApplicationName"];
            Common.DefaultSettingsStorage.AdminEmail = configuration.GetSection("FirstAppStart")["AdminEmail"];
            Common.DefaultSettingsStorage.AdminPassword = configuration.GetSection("FirstAppStart")["AdminPassword"];
            Common.DefaultSettingsStorage.SenderEmail = configuration.GetSection("DefaultEmail")["Sender"];
            Common.DefaultSettingsStorage.ReceiverEmail = configuration.GetSection("DefaultEmail")["Receiver"];
            Common.DefaultSettingsStorage.CronForSendEmailJob = configuration.GetSection("Quartz")["SendEmailJob"];
            Common.DefaultSettingsStorage.SendEmailJobIfDays = configuration.GetSection("Quartz")["SendEmailJobIfDays"];
        }

        public static void SetSupportedCulture(this IServiceCollection services)
        {
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ru"),
                };

                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
        }

    }
}

