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

        public string GetConnectionString()
        {
            var appConfig = System.Configuration.ConfigurationManager.AppSettings;

            string dbname = appConfig["RDS_DB_NAME"];

            if (string.IsNullOrEmpty(dbname)) return _appConfiguration.GetConnectionString(uvahoyConsts.ConnectionStringName);

            string username = appConfig["RDS_USERNAME"];
            string password = appConfig["RDS_PASSWORD"];
            string hostname = appConfig["RDS_HOSTNAME"];
            string port = appConfig["RDS_PORT"];

            return "Data Source=" + hostname + ";DataBase=" + dbname + ";User ID=" + username + ";Password=" + password + ";";
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = GetConnectionString();

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
