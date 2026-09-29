using System.ComponentModel.DataAnnotations;

namespace Proyecto_Tesis.Models.DTO
{
    public sealed class LatenciaDto
    {
        [Range(1, 60000)]
        public double LatenciaMs { get; set; }
    }
}
