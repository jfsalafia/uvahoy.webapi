using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace uvahoy.Indicadores.Dto
{
    [AutoMapFrom(typeof(Indicador))]
    public class IndicadorListDto : FullAuditedEntityDto
    {
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public string Abreviatura { get; set; }
    }
}