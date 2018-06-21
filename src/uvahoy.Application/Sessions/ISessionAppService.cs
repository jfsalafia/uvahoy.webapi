using System.Threading.Tasks;
using Abp.Application.Services;
using uvahoy.Sessions.Dto;

namespace uvahoy.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
