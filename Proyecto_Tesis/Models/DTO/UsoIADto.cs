using System.ComponentModel.DataAnnotations;

namespace Proyecto_Tesis.Models.DTO
{
    public sealed class UsoIADto
    {
        [Range(0, 24)]
        public int HorasUsoDiario { get; set; }

        [Range(1, 7)]
        public int DiasSemana { get; set; }

        [Required]
        [RegularExpression("^(Apoyo|Reemplazo)$")]
        public string TipoUso { get; set; } = string.Empty;
    }
}
