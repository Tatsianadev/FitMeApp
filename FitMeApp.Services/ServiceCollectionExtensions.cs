using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FitMeApp.Repository.EntityFramework;
using FitMeApp.Common;
using FitMeApp.Repository.EntityFramework.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Interfaces;
using Microsoft.Extensions.Configuration;

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
            services.AddScoped<IFileStorage, FileStorage>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IExcelReport, ExcelReportEpPlus>();
            //services.AddScoped<IExcelReport, ExcelReportOpenXml>();
        }

        public static void RegisterMailService(this IServiceCollection services, string apiKey)
        {
            services.AddSingleton<IEmailService>(emailService => new EmailService(apiKey));
        }

        public static void InitializeDefaultSettings(this IServiceCollection services, IConfiguration configuration)
        {
            Common.DefaultSettingsStorage.AvatarPath = configuration.GetSection("Constants")["DefaultAvatarPath"];
            Common.DefaultSettingsStorage.ApplicationName = configuration.GetSection("Constants")["ApplicationName"];
            Common.DefaultSettingsStorage.AdminEmail = configuration.GetSection("FirstAppStart")["AdminEmail"];
            Common.DefaultSettingsStorage.AdminPassword = configuration.GetSection("FirstAppStart")["AdminPassword"];
            Common.DefaultSettingsStorage.SenderEmail = configuration.GetSection("DefaultEmail")["Sender"];
            Common.DefaultSettingsStorage.ReceiverEmail = configuration.GetSection("DefaultEmail")["Receiver"];
            Common.DefaultSettingsStorage.ApproveAppMessagePath = configuration.GetSection("Constants")["ApproveAppMessagePath"];
            Common.DefaultSettingsStorage.RejectAppMessagePath = configuration.GetSection("Constants")["RejectAppMessagePath"];
        }

    }
}

