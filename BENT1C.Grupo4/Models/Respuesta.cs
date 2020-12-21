using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BENT1C.Grupo4.Models
{
    public class Respuesta
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(250, ErrorMessage = "El campo {0} admite un maximo de {1} caracteres")]
        public string Descripcion { get; set; }

        public DateTime Fecha { get; set; }

        [ForeignKey(nameof(Pregunta))]
        public Guid PreguntaId { get; set; }
        public Pregunta Pregunta { get; set; }

        [ForeignKey(nameof(Miembro))]
        public Guid MiembroId { get; set; }
        public Miembro Miembro { get; set; }

        public List<Like> Reacciones { get; set; }
    }
}
