using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FitMeApp.Repository.EntityFramework;
using FitMeApp.Common;
using FitMeApp.Contracts;
using FitMeApp.Services.Mapping;
using FitMeApp.Contracts.Interfaces;

namespace FitMeApp.Services
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterDbContext(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(DbConnecntion.GetConnectionString()));
        }

        public static void RegisterIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                //options.User.RequireUniqueEmail = true;
                
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();            
        }

        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddScoped<IRepository, Repository.EntityFramework.Repository>();
            services.AddScoped<IMapper, Mapper>();
        }
    }
}

