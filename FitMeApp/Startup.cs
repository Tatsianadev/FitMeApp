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
            var connectionString = Configuration.GetConnectionString("Default");
            services.RegisterDbContext(connectionString);
            services.RegisterIdentity();
            services.RegisterDependencies();
            services.AddControllersWithViews();
            services.AddSignalR();

            var apiKey = Configuration.GetSection("SendGrid")["ApiKey"];
            services.RegisterMailService(apiKey);

            //todo set all constants this way 
            Common.DefaultSettingsStorage.AvatarPath = Configuration.GetSection("Constants")["AvatarPath"];
            Common.DefaultSettingsStorage.ApplicationName = Configuration.GetSection("Constants")["ApplicationName"];
            Common.DefaultSettingsStorage.AdminEmail = Configuration.GetSection("FirstAppStart")["AdminEmail"];
            Common.DefaultSettingsStorage.AdminPassword = Configuration.GetSection("FirstAppStart")["AdminPassword"];
            Common.DefaultSettingsStorage.SenderEmail = Configuration.GetSection("DefaultEmail")["Sender"];
            Common.DefaultSettingsStorage.ReceiverEmail = Configuration.GetSection("DefaultEmail")["Receiver"];

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); //added
            app.UseAuthorization();

            loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHub<ChatHub>("/chat");  //added to chat

            });
        }
    }
}
