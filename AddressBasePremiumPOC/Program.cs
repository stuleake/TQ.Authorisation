using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using TQ.Geocoding.DataLoad.Models.Context;

namespace TQ.Geocoding.DataLoad
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();

            ////var host = CreateWebHostBuilder(args).Build();

            ////using (var scope = host.Services.CreateScope())
            ////{
            ////    var services = scope.ServiceProvider;

            ////    try
            ////    {
            ////        var context = services.GetRequiredService<AddressBasePremiumContext>();
            ////        context.Database.EnsureCreated();
            ////    }
            ////    catch (Exception ex)
            ////    {
            ////        var logger = services.GetRequiredService<ILogger<Program>>();
            ////        logger.LogError(ex, "An error occurred creating the DB.");
            ////    }
            ////}

            ////host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddEventSourceLogger();
                })
                .UseStartup<Startup>();
    }
}
