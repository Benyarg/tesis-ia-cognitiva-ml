namespace Proyecto_Tesis.ViewModels.Admin;

public sealed class ParticipanteResumenViewModel
{
    public int Id { get; init; }
    public string Codigo { get; init; } = string.Empty;
    public DateTime FechaRegistro { get; init; }
    public bool Consentimiento { get; init; }
    public bool Cognitivo { get; init; }
    public bool Emocional { get; init; }
    public bool UsoIA { get; init; }
    public bool Sus { get; init; }
    public string Estado { get; init; } = "Pendiente";
    public string Nivel { get; init; } = "Sin resultado";
}
