﻿using System;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.Configuration;
using uvahoy.Authentication.JwtBearer;
using uvahoy.Configuration;
using uvahoy.EntityFrameworkCore;

#if FEATURE_SIGNALR
using Abp.Web.SignalR;
#elif FEATURE_SIGNALR_ASPNETCORE
using Abp.AspNetCore.SignalR;
#endif

namespace uvahoy
{
    [DependsOn(
         typeof(uvahoyApplicationModule),
         typeof(uvahoyEntityFrameworkModule),
         typeof(AbpAspNetCoreModule)
#if FEATURE_SIGNALR 
        ,typeof(AbpWebSignalRModule)
#elif FEATURE_SIGNALR_ASPNETCORE
        , typeof(AbpAspNetCoreSignalRModule)
#endif
     )]
    public class uvahoyWebCoreModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public uvahoyWebCoreModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
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

            if (string.IsNullOrEmpty(dbname))
                return _appConfiguration.GetConnectionString(uvahoyConsts.ConnectionStringName);

            string username = GetEnvironmentConfig("RDS_USERNAME");
            string password = GetEnvironmentConfig("RDS_PASSWORD");
            string hostname = GetEnvironmentConfig("RDS_HOSTNAME");
            string port = GetEnvironmentConfig("RDS_PORT");

            return "Data Source=" + hostname + ";Database=" + dbname + ";User ID=" + username + ";Password=" + password + ";";
        }


        public override void PreInitialize()
        {

            Configuration.DefaultNameOrConnectionString = GetConnectionString();

            // Use database for language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            Configuration.Modules.AbpAspNetCore()
                 .CreateControllersForAppServices(
                     typeof(uvahoyApplicationModule).GetAssembly()
                 );

            ConfigureTokenAuth();
        }

        private void ConfigureTokenAuth()
        {
            IocManager.Register<TokenAuthConfiguration>();
            var tokenAuthConfig = IocManager.Resolve<TokenAuthConfiguration>();

            tokenAuthConfig.SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appConfiguration["Authentication:JwtBearer:SecurityKey"]));
            tokenAuthConfig.Issuer = _appConfiguration["Authentication:JwtBearer:Issuer"];
            tokenAuthConfig.Audience = _appConfiguration["Authentication:JwtBearer:Audience"];
            tokenAuthConfig.SigningCredentials = new SigningCredentials(tokenAuthConfig.SecurityKey, SecurityAlgorithms.HmacSha256);
            tokenAuthConfig.Expiration = TimeSpan.FromDays(1);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(uvahoyWebCoreModule).GetAssembly());
        }
    }
}
