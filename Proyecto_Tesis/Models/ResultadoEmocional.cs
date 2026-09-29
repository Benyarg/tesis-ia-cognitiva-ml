namespace Proyecto_Tesis.Models
{
    public class ResultadoEmocional
    {
        public int Id { get; set; }
        public string UserToken { get; set; } = string.Empty;

        public int Estres { get; set; }
        public int Ansiedad { get; set; }
        public int Depresion { get; set; }

        public DateTime Fecha { get; set; }
    }
}
