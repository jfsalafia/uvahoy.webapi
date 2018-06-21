using System.Threading.Tasks;
using uvahoy.Configuration.Dto;

namespace uvahoy.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
