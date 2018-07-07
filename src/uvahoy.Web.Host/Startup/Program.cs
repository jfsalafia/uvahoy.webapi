using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

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
            return WebHost.CreateDefaultBuilder(args)
                 .UseKestrel()
                .UseContentRoot(System.IO.Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .CaptureStartupErrors(true)
                .Build();
        }
    }
}
