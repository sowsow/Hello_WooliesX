using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace WooliesX.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(
                    (hostingContext, configBuilder) =>
                    {
                        configBuilder
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("UserProfile.json");
                    })
                .UseStartup<Startup>();
    }
}
