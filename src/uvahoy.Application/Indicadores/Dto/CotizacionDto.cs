using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;

namespace uvahoy.Indicadores.Dto
{
    [AutoMapFrom(typeof(Cotizacion))]
    public class CotizacionDto : CreationAuditedEntityDto
    {
        public virtual int IndicadorId { get; set; }

        public virtual DateTime FechaHoraCotizacion { get; set; }

        public virtual decimal ValorCotizacion { get; set; }
    }
}