using Abp.Application.Services;
using Abp.Application.Services.Dto;
using uvahoy.MultiTenancy.Dto;

namespace uvahoy.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}
