namespace Proyecto_Tesis.ViewModels.Admin;

public sealed class AdminDashboardViewModel
{
    public int Participantes { get; init; }
    public int ConsentimientosAceptados { get; init; }
    public int EvaluacionesCompletas { get; init; }
    public int PrediccionesGeneradas { get; init; }
    public int NivelAlto { get; init; }
    public int NivelBajo { get; init; }
    public ResultadoMlResumen? UltimoModelo { get; init; }
    public IReadOnlyList<ParticipanteResumenViewModel> Recientes { get; init; } = [];
}

public sealed class ResultadoMlResumen
{
    public float Accuracy { get; init; }
    public float Precision { get; init; }
    public float Recall { get; init; }
    public float F1 { get; init; }
    public DateTime Fecha { get; init; }
}
