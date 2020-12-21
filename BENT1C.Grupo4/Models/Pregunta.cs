using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BENT1C.Grupo4.Models
{
    public class Pregunta
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(500, ErrorMessage = "El campo {0} admite un maximo de {1} caracteres")]
        public string Descripcion { get; set; }

        public DateTime Fecha { get; set; }

        public bool Activa { get; set; }

        [ForeignKey(nameof(Entrada))]
        public Guid EntradaId { get; set; }
        public Entrada Entrada { get; set; }

        [ForeignKey(nameof(Miembro))]
        public Guid MiembroId { get; set; }
        public Miembro Miembro { get; set; }

        public List<Respuesta> Respuestas { get; set; }

    }
}
