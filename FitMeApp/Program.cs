using FitMeApp.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace FitMeApp
{
    public class Program
    {
        //public static void Main(string[] args)
        //{
        //    CreateHostBuilder(args).Build().Run();
        //}

        public static async Task Main(string[] args)
        {

            try
            {
                var host = CreateHostBuilder(args).Build();
                await ServiceProviderExtensions.RoleInitializeAsync(host);

                await host.RunAsync();
            }
            catch (Exception ex)
            {
                var loggerFactory = CreateHostBuilder(args).Build().Services.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger(nameof(Main));
                logger.LogError(nameof(Main), ex.Message);
               
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    //var env = hostingContext.HostingEnvironment;

                    config.AddJsonFile("appsettings.json", optional: true)
                        .AddJsonFile("appsettings.local.json", optional: true);

                    config.AddEnvironmentVariables();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
