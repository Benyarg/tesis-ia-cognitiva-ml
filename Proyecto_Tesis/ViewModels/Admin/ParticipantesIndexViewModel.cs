namespace Proyecto_Tesis.ViewModels.Admin;

public sealed class ParticipantesIndexViewModel
{
    public string Busqueda { get; init; } = string.Empty;
    public IReadOnlyList<ParticipanteResumenViewModel> Participantes { get; init; } = [];
    public int Pagina { get; init; } = 1;
    public int TotalPaginas { get; init; } = 1;
    public int TotalRegistros { get; init; }
}
