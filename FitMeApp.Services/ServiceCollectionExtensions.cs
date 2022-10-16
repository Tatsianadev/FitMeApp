using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FitMeApp.Repository;
using FitMeApp.Common;
using FitMeApp.Contracts;

namespace FitMeApp.Services
{
    public static class ServiceCollectionExtensions
    {
        public static void AddConfigureServices(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(ConnectionDb.GetConnectionString()));
            //services.AddIdentityCore<User>()
            //        .AddRoles<IdentityRole>()
            //        .AddEntityFrameworkStores<ApplicationDbContext>();
            //.AddSignInManager<SignInManager<User>>();
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
        }
    }
}
