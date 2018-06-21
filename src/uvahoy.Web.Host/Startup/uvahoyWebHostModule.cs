using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using uvahoy.Configuration;

namespace uvahoy.Web.Host.Startup
{
    [DependsOn(
       typeof(uvahoyWebCoreModule))]
    public class uvahoyWebHostModule: AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public uvahoyWebHostModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(uvahoyWebHostModule).GetAssembly());
        }
    }
}
