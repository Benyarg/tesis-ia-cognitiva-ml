namespace Proyecto_Tesis.Models
{
    public class Consentimiento
    {
        public int Id { get; set; }
        public string UserToken { get; set; } = string.Empty;
        public bool Aceptado { get; set; }
        public DateTime Fecha { get; set; }
    }
}