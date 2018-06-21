using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using uvahoy.Configuration.Dto;

namespace uvahoy.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : uvahoyAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
