using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace uvahoy.Web.Host.Startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var config = new ConfigurationBuilder().AddCommandLine(args).Build();
            return WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseContentRoot(System.IO.Directory.GetCurrentDirectory())
                .UseConfiguration(config)
                .UseIISIntegration()
                .CaptureStartupErrors(true)
                .UseStartup<Startup>()
                .Build();
        }
    }
}
