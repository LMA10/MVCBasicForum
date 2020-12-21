using System;
using System.ComponentModel.DataAnnotations;

namespace BENT1C.Grupo4.Models
{
    public abstract class Usuario
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El campo {0} admite un maximo de {1} caracteres")]
        [MinLength(2, ErrorMessage = "El campo {0} admite un minimo de {1} caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El campo {0} admite un maximo de {1} caracteres")]
        [MinLength(2, ErrorMessage = "El campo {0} admite un minimo de {1} caracteres")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(75, ErrorMessage = "El campo {0} admite un maximo de {1} caracteres")]
        [EmailAddress(ErrorMessage = "El campo {0} debe ser un email")]
        public string Email { get; set; }
        public DateTime FechaAlta { get; set; }

        // La password la tratamos como un array de bytes ya que va a ir encriptada
        [ScaffoldColumn(false)] // agregamos [ScaffoldColumn(false)] que hace que no se incluya esta propiedad en el scaffold
        public byte[] Password { get; set; }

        public abstract Rol Rol { get; }
    }
}
