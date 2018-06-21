using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using uvahoy.Authorization;

namespace uvahoy
{
    [DependsOn(
        typeof(uvahoyCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class uvahoyApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<uvahoyAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(uvahoyApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddProfiles(thisAssembly)
            );
        }
    }
}
