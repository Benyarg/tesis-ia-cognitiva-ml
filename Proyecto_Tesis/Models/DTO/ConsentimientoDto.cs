using System.ComponentModel.DataAnnotations;

namespace Proyecto_Tesis.Models.DTO
{
    public sealed class ConsentimientoDto
    {
        [Required]
        public bool Aceptado { get; set; }
    }
}
