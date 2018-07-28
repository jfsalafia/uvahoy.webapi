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


        private static string GetEnvironmentConfig(string variable)
        {
            if (!string.IsNullOrEmpty(System.Environment.GetEnvironmentVariable(variable)))
            {
                return System.Environment.GetEnvironmentVariable(variable);
            }
            return System.Configuration.ConfigurationManager.AppSettings[variable];
        }

        public string GetConnectionString()
        {
            string dbname = GetEnvironmentConfig("RDS_DB_NAME");

            if (string.IsNullOrEmpty(dbname)) return _appConfiguration.GetConnectionString(uvahoyConsts.ConnectionStringName);

            string username = GetEnvironmentConfig("RDS_USERNAME");
            string password = GetEnvironmentConfig("RDS_PASSWORD");
            string hostname = GetEnvironmentConfig("RDS_HOSTNAME");
            string port = GetEnvironmentConfig("RDS_PORT");

            return "Data Source=" + hostname + ";Database=" + dbname + ";User ID=" + username + ";Password=" + password + ";";
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
