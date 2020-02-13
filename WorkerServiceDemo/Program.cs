using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using WorkerService.Service;

namespace WorkerServiceDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // once it become a background service. It won't have the debug window. So we have to log it in txt file.
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File(@"C:\Data\Code\VS2019\WorkerServiceDemo\LogFile.txt")
                .CreateLogger();

            try
            {
                Log.Information("Starting up the service");
                CreateHostBuilder(args).Build().Run();
                return;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "There was a problem starting the service");
                return;
            }
            finally
            {
                // ensure any message in the buffer will write into log. not disappear.
                Log.CloseAndFlush();
            }
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    // get config
                    services.Configure<AppSettings>(hostContext.Configuration.GetSection("AppSettings"));
                    // set up with DI
                    services.AddScoped<IServiceA, ServiceA>();
                    services.AddScoped<IServiceB, ServiceB>();
                })
                .UseSerilog();
    }
}
