using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace uvahoy.Indicadores.Dto
{
    [AutoMapFrom(typeof(Indicador))]
    public class IndicadorDetailOutput : FullAuditedEntityDto<int>
    {
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public ICollection<CotizacionDto> Cotizaciones { get; set; }
    }

    public class MultiIndicadorDetailOutput
    {
        public Dictionary<int, string> Nombres { get; set; }

        public Dictionary<int, string> Descripciones { get; set; }

        public ICollection<CotizacionDto> Cotizaciones { get; set; }
    }

    public class IndicadorDetailInput : IValidatableObject
    {
  
        public int IndicadorId { get; set; }

        public DateTime FechaDesde { get; set; }

        public DateTime? FechaHasta { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {


            yield return ValidationResult.Success;


        }
    }

    public class MultiIndicadorDetailInput : IValidatableObject
    {
        public string Indicadores { get; set; }

        public string Fechas { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return ValidationResult.Success;
        }
    }
}
