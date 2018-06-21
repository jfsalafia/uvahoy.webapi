using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace uvahoy.Indicadores
{
    [Table("Cotizacion")]
    public class Cotizacion : Entity
    {
        [ForeignKey("IndicadorId")]
        public virtual Indicador Indicador { get; protected set; }

        public virtual int IndicadorId { get; protected set; }


        [Required]
        public virtual DateTime FechaHoraCotizacion { get; protected set; }

        public virtual decimal? ValorCotizacion { get; protected set; }

        public bool SinCotizacion
        {
            get
            {
                return !ValorCotizacion.HasValue;
            }
        }

        public static Cotizacion Create(int indicadorId, DateTime ini, decimal valorCotizacion)
        {
            return new Cotizacion() { IndicadorId = indicadorId, FechaHoraCotizacion = ini, ValorCotizacion = valorCotizacion };
        }
    }
}
