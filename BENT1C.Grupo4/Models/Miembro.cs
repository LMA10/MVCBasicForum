using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BENT1C.Grupo4.Models
{
    public class Miembro : Usuario
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(15, ErrorMessage = "El campo {0} admite un maximo de {1} caracteres")]
        [RegularExpression(@"[0-9]{3}[0-9]{4}[0-9]{4}", ErrorMessage = "Ingrese un telefono valido.")]
        public string Telefono { get; set; }
        public List<Entrada> Entradas { get; set; }
        public List<Pregunta> Preguntas { get; set; }
        public List<Respuesta> Respuestas { get; set; }
        public List<Like> RespuestasQueMeGustan { get; set; }
        public List<EntradaMiembro> EntradasHabilitadas { get; set; }

        public override Rol Rol => Rol.Miembro;
    }
}
