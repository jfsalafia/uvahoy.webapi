using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace uvahoy.Indicadores
{
    [Table("Indicador")]
    public class Indicador : Entity, ISoftDelete
    {
        [Required]
        [StringLength(uvahoyConsts.MaxNombreLength)]
        public virtual string Nombre { get; protected set; }

        [Required]
        public virtual string FuenteDatos { get; protected set; }

        [StringLength(uvahoyConsts.MaxDescripcionLength)]
        public virtual string Descripcion { get; protected set; }

        public virtual string Abreviatura { get; protected set; }

        public virtual bool Activo { get; protected set; }
        
        public virtual string MetodoActualizacion {get; set;}

        public virtual string FormatoDatos { get; set; }

        public bool IsDeleted
        {
            get
            {
                return Activo;
            }
            set
            {
                Activo = value;
            }
        }
        
        public static Indicador Create(string nombre, string fuenteDatos, string descripcion, string abreviatura)
        {
            return new Indicador()
            {

                Nombre = nombre,
                FuenteDatos = fuenteDatos,
                Descripcion = descripcion,
                Abreviatura = abreviatura,
                Activo = true
            };
        }
       
    }
}
