using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PackageWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;
                    string cs = hostContext.HostingEnvironment.IsDevelopment() ? configuration.GetConnectionString("Local") : configuration.GetConnectionString("Server");                   
                    services.AddSingleton((prov) => new TrackerDAL.TrackerSystem(cs));
                    services.AddSingleton((prov) => new TrackerDAL.Tracking(cs));
                    services.Configure<Models.PushNotification>(configuration.GetSection("PushNotification"));
                    services.Configure<Models.EmailSettings>(configuration.GetSection("EmailSettings"));
                    services.AddHostedService<Worker>();
                });
    }
}
