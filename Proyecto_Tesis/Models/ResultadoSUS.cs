namespace Proyecto_Tesis.Models
{
    public class ResultadoSUS
    {
        public int Id { get; set; }
        public string UserToken { get; set; } = string.Empty;

        public double PuntajeTotal { get; set; }

        public DateTime Fecha { get; set; }
    }
}
