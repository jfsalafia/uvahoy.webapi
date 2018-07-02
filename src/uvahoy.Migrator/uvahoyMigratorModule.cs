using Microsoft.Extensions.Configuration;
using Castle.MicroKernel.Registration;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using uvahoy.Configuration;
using uvahoy.EntityFrameworkCore;
using uvahoy.Migrator.DependencyInjection;

namespace uvahoy.Migrator
{
    [DependsOn(typeof(uvahoyEntityFrameworkModule))]
    public class uvahoyMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public uvahoyMigratorModule(uvahoyEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(uvahoyMigratorModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public override void PreInitialize()
        {
            string cnnString = _appConfiguration.GetConnectionString(uvahoyConsts.ConnectionStringName);

            if (System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
                cnnString = System.Environment.GetEnvironmentVariable("defaultConnection");


            Configuration.DefaultNameOrConnectionString = cnnString;

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(
                typeof(IEventBus), 
                () => IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                )
            );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(uvahoyMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}
