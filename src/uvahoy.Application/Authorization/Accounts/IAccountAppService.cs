using System.Threading.Tasks;
using Abp.Application.Services;
using uvahoy.Authorization.Accounts.Dto;

namespace uvahoy.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
