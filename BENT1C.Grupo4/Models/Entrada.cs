using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BENT1C.Grupo4.Models
{
    public class Entrada
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El campo {0} admite un maximo de {1} caracteres")]
        [MinLength(1, ErrorMessage = "El campo {0} admite un minimo de {1} caracteres")]
        public string Titulo { get; set; }

        public DateTime Fecha { get; set; }
        
        public bool Privada { get; set; }


        [ForeignKey(nameof(Categoria))]
        public Guid CategoriaId { get; set; }
        public Categoria Categoria { get; set; }


        [ForeignKey(nameof(Miembro))]
        public Guid MiembroId { get; set; }
        public Miembro Miembro { get; set; }


        public List<Pregunta> Preguntas { get; set; }

        public List<EntradaMiembro> MiembrosHabilitados { get; set; }
    }
}
