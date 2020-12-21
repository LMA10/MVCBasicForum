using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BENT1C.Grupo4.Models
{
    public class Categoria
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(30, ErrorMessage = "El campo {0} admite un maximo de {1} caracteres")]
        public string Nombre { get; set; }
        public List<Entrada> Entradas { get; set; }
    }
}
