namespace Proyecto_Tesis.ViewModels.Admin;

public sealed class ParticipanteDetalleViewModel
{
    public int Id { get; init; }
    public string Codigo { get; init; } = string.Empty;
    public DateTime FechaRegistro { get; init; }
    public bool ConsentimientoAceptado { get; init; }
    public DateTime? FechaConsentimiento { get; init; }
    public double? LatenciaMs { get; init; }
    public DateTime? FechaCognitivo { get; init; }
    public int? Estres { get; init; }
    public int? Ansiedad { get; init; }
    public int? Depresion { get; init; }
    public DateTime? FechaEmocional { get; init; }
    public int? HorasUsoDiario { get; init; }
    public int? DiasSemana { get; init; }
    public string? TipoUso { get; init; }
    public DateTime? FechaUsoIA { get; init; }
    public double? PuntajeSus { get; init; }
    public DateTime? FechaSus { get; init; }
    public string Nivel { get; init; } = "Sin resultado";
    public DateTime? FechaPrediccion { get; init; }
}
