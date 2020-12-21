using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BENT1C.Grupo4.Models
{
    public class EntradaMiembro
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Entrada))]
        public Guid IdEntrada { get; set; }
        public Entrada Entrada { get; set; }

        [ForeignKey(nameof(Miembro))]
        public Guid IdMiembro { get; set; }
        public Miembro Miembro { get; set; }

        public bool Habilitado { get; set; }
    }
}
