using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BENT1C.Grupo4.Models
{
    public class Like
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime Fecha { get; set; }

        public bool MeGusta { get; set; }

        [ForeignKey(nameof(Respuesta))]
        public Guid RespuestaId { get; set; }
        public Respuesta Respuesta { get; set; }

        [ForeignKey(nameof(Miembro))]
        public Guid MiembroId { get; set; }
        public Miembro Miembro { get; set; }
    }
}
