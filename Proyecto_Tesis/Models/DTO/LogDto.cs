using System.ComponentModel.DataAnnotations;

namespace Proyecto_Tesis.Models.DTO
{
    public sealed class LogDto
    {
        [Required]
        [StringLength(80)]
        public string ActionType { get; set; } = string.Empty;

        [Range(0, 86400000)]
        public double LatencyMs { get; set; }

        [Range(0, 100000)]
        public int InputSize { get; set; }
    }
}
