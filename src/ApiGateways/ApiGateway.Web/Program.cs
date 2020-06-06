using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Ocelot.DependencyInjection;
using System.IO;

namespace Apotheek.Base.ApiGateway.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        public static IWebHostBuilder CreateHostBuilder(string[] args) =>
            new WebHostBuilder()
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config
                    .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                    .AddJsonFile("appsettings.json", true, true)
                    .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                    .AddOcelot($"{hostingContext.HostingEnvironment.ContentRootPath}{Path.DirectorySeparatorChar}RoutingConfigurations{Path.DirectorySeparatorChar}{hostingContext.HostingEnvironment.EnvironmentName}", hostingContext.HostingEnvironment)
                    .AddEnvironmentVariables();
            })
            //.ConfigureLogging((hostingContext, logging) =>
            //{
            //    logging.AddConsole();
            //})
            .UseStartup<Startup>();
    }
}
