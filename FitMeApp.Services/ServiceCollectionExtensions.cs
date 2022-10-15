using FitMeApp.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FitMeApp.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using FitMeApp.Common;



namespace FitMeApp.Services
{
    public static class ServiceCollectionExtensions
    {
        public static void AddConfigureServices(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(ConnectionDb.GetConnectionString()));
            services.AddIdentityCore<User>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();
        }
    }
}
