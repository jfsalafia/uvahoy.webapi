using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using uvahoy.Indicadores.Dto;

namespace uvahoy.Indicadores
{
    public interface IIndicadorAppService : IApplicationService
    {
        Task CreateAsync(CreateIndicadorInput input);
        
        IndicadorDetailOutput GetIndicadorDetail(IndicadorDetailInput input);

        ListResultDto<IndicadorListDto> GetList(GetIndicadorListInput input);

        Task RegistrarIndicadorUsuario(EntityDto input);

        MultiIndicadorDetailOutput GetMultiIndicadorDetail(MultiIndicadorDetailInput input);
    }
}
