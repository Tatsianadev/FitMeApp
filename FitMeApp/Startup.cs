using System.Globalization;
using FitMeApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using FitMeApp.Common.FileLogging;
using System.IO;
using FitMeApp.Chat;
using FitMeApp.Common;
using FitMeApp.Resources;
using Microsoft.AspNetCore.Localization;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;


namespace FitMeApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.InitializeDefaultSettings(Configuration);
            services.RegisterSchedulerService();

            var connectionString = Configuration.GetConnectionString("Default");
            services.RegisterDbContext(connectionString);
            services.RegisterIdentity();
            services.RegisterDependencies();
            services.RegisterCacheService();
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddControllersWithViews();
            services.SetSupportedCulture();
            services.AddSignalR();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "FitMeApp v1", Version = "v1"});
            });

            var apiKey = Configuration.GetSection("SendGrid")["ApiKey"];
            services.RegisterMailService(apiKey);
            
            // Connect to docker IP
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("Redis"); //"localhost:6379"
                options.InstanceName = "FitMeApp_";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FitMeApp v1");
                    c.RoutePrefix = "Swagger";
                });

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseRequestLocalization();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHub<ChatHub>("/chat"); 

            });
        }
    }
}
