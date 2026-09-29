namespace Proyecto_Tesis.Models
{
    public class ResultadoPrediccion
    {
        public int Id { get; set; }

        public string UserToken { get; set; } = string.Empty;

        public string NivelRiesgo { get; set; } = string.Empty; // ELEVADO / BAJO

        public float Probabilidad { get; set; }

        public DateTime Fecha { get; set; }
    }
}