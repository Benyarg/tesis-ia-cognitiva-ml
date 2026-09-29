using System.ComponentModel.DataAnnotations;

namespace Proyecto_Tesis.Models.DTO
{
    public sealed class DassDto
    {
        [Range(0, 21)]
        public int Estres { get; set; }

        [Range(0, 21)]
        public int Ansiedad { get; set; }

        [Range(0, 21)]
        public int Depresion { get; set; }
    }
}
