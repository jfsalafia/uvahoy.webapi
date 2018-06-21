using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace uvahoy.Indicadores.Dto
{
    public class CreateIndicadorInput : IValidatableObject
    {
        [Required]
        [StringLength(uvahoyConsts.MaxNombreLength)]
        public string Nombre { get; set; }

        [Required]
        public string FuenteDatos { get; set; }

        [Required]
        [StringLength(uvahoyConsts.MaxDescripcionLength)]
        public string Descripcion { get; set; }

        public string Abreviatura { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return ValidationResult.Success;
        }
    }
}
