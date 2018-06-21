using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;

namespace uvahoy.Indicadores.Dto
{
    [AutoMapFrom(typeof(Cotizacion))]
    public class CotizacionDto : CreationAuditedEntityDto
    {
        public virtual int IndicadorId { get; protected set; }

        public virtual DateTime FechaHoraCotizacion { get; protected set; }

        public virtual decimal ValorCotizacion { get; protected set; }
    }
}