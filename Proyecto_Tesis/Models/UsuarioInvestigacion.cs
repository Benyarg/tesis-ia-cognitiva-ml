namespace Proyecto_Tesis.Models
{
    public class UsuarioInvestigacion
    {
        public int Id { get; set; }
        public string TokenHash { get; set; } = string.Empty; // Identificador aleatorio de investigación
        public DateTime FechaRegistro { get; set; }
    }
}
