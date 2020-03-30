using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace LambdaForums.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((builderContext, config) => 
            {
                IHostingEnvironment env = builderContext.HostingEnvironment;
                config.AddJsonFile("storageSettings.json", optional: false, reloadOnChange: true);
            })
                .UseStartup<Startup>();
    }
}
