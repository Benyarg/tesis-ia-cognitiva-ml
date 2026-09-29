using System.ComponentModel.DataAnnotations;

namespace Proyecto_Tesis.Models.DTO
{
    public sealed class SusDto
    {
        [Range(0, 100)]
        public double PuntajeTotal { get; set; }
    }
}
