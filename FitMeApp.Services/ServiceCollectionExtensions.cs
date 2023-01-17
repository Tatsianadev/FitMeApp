﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FitMeApp.Repository.EntityFramework;
using FitMeApp.Common;
using FitMeApp.Repository.EntityFramework.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Interfaces;
using Microsoft.Extensions.Logging;
using FitMeApp.Common.FileLogging;
using Microsoft.AspNetCore.SignalR;

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
                .AddEntityFrameworkStores<ApplicationDbContext>();            
        }

        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddScoped<IRepository, Repository.EntityFramework.Repository>();
            services.AddScoped<IFitMeService, FitMeService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IChatService, ChatService>();

        }
    }
}

