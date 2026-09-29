namespace Proyecto_Tesis.Models
{
    public class UsoIA
    {
        public int Id { get; set; }
        public string UserToken { get; set; } = string.Empty;

        public int HorasUsoDiario { get; set; }
        public int DiasSemana { get; set; }
        public string TipoUso { get; set; } = string.Empty; // Apoyo / Reemplazo
        public DateTime Fecha { get; set; }
    }
}
