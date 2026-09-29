using Microsoft.EntityFrameworkCore;
using Proyecto_Tesis.Data;
using Proyecto_Tesis.Models;
using Proyecto_Tesis.ViewModels.Admin;

namespace Proyecto_Tesis.Services;

public interface IAdminDashboardService
{
    Task<AdminDashboardViewModel> ObtenerDashboardAsync();
    Task<ParticipantesIndexViewModel> ObtenerParticipantesAsync(string? busqueda, int pagina = 1);
    Task<ParticipanteDetalleViewModel?> ObtenerDetalleAsync(int id);
}

public sealed class AdminDashboardService : IAdminDashboardService
{
    private readonly ApplicationDbContext _context;

    public AdminDashboardService(ApplicationDbContext context) => _context = context;

    public async Task<AdminDashboardViewModel> ObtenerDashboardAsync()
    {
        var usuariosRecientes = await _context.UsuariosInvestigacion.AsNoTracking()
            .OrderByDescending(x => x.FechaRegistro).Take(8).ToListAsync();
        var recientes = await CrearResumenesAsync(usuariosRecientes);

        var predicciones = await _context.ResultadosPrediccion.AsNoTracking().ToListAsync();
        var ultimasPredicciones = predicciones.GroupBy(x => x.UserToken)
            .Select(g => g.OrderByDescending(x => x.Fecha).First()).ToList();
        var ultimoModelo = await _context.ResultadosML.AsNoTracking().OrderByDescending(x => x.Fecha).FirstOrDefaultAsync();

        var cognitivoTokens = await _context.ResultadosCognitivos.AsNoTracking().Select(x => x.UserToken).Distinct().ToListAsync();
        var emocionalTokens = await _context.ResultadosEmocionales.AsNoTracking().Select(x => x.UserToken).Distinct().ToListAsync();
        var usoTokens = await _context.UsoIA.AsNoTracking().Select(x => x.UserToken).Distinct().ToListAsync();
        var susTokens = await _context.ResultadosSUS.AsNoTracking().Select(x => x.UserToken).Distinct().ToListAsync();
        var completos = cognitivoTokens.Intersect(emocionalTokens).Intersect(usoTokens).Intersect(susTokens).Count();

        return new AdminDashboardViewModel
        {
            Participantes = await _context.UsuariosInvestigacion.CountAsync(),
            ConsentimientosAceptados = await _context.Consentimientos.CountAsync(x => x.Aceptado),
            EvaluacionesCompletas = completos,
            PrediccionesGeneradas = ultimasPredicciones.Count,
            NivelAlto = ultimasPredicciones.Count(x => NormalizarNivel(x.NivelRiesgo) == "ALTO"),
            NivelBajo = ultimasPredicciones.Count(x => NormalizarNivel(x.NivelRiesgo) == "BAJO"),
            UltimoModelo = ultimoModelo is null ? null : new ResultadoMlResumen
            {
                Accuracy = ultimoModelo.Accuracy,
                Precision = ultimoModelo.Precision,
                Recall = ultimoModelo.Recall,
                F1 = ultimoModelo.F1Score,
                Fecha = ultimoModelo.Fecha
            },
            Recientes = recientes
        };
    }

    public async Task<ParticipantesIndexViewModel> ObtenerParticipantesAsync(string? busqueda, int pagina = 1)
    {
        const int pageSize = 25;
        pagina = Math.Max(1, pagina);
        var q = busqueda?.Trim() ?? string.Empty;
        IQueryable<UsuarioInvestigacion> query = _context.UsuariosInvestigacion.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var numeric = q.Replace("P-", string.Empty, StringComparison.OrdinalIgnoreCase).Trim();
            if (int.TryParse(numeric, out var id)) query = query.Where(x => x.Id == id);
            else query = query.Where(x => false);
        }

        var total = await query.CountAsync();
        var totalPages = Math.Max(1, (int)Math.Ceiling(total / (double)pageSize));
        pagina = Math.Min(pagina, totalPages);
        var usuarios = await query.OrderByDescending(x => x.FechaRegistro)
            .Skip((pagina - 1) * pageSize).Take(pageSize).ToListAsync();

        return new ParticipantesIndexViewModel
        {
            Busqueda = q,
            Participantes = await CrearResumenesAsync(usuarios),
            Pagina = pagina,
            TotalPaginas = totalPages,
            TotalRegistros = total
        };
    }

    public async Task<ParticipanteDetalleViewModel?> ObtenerDetalleAsync(int id)
    {
        var usuario = await _context.UsuariosInvestigacion.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (usuario is null) return null;

        var token = usuario.TokenHash;
        var consentimiento = await _context.Consentimientos.AsNoTracking().Where(x => x.UserToken == token).OrderByDescending(x => x.Fecha).FirstOrDefaultAsync();
        var cognitivo = await _context.ResultadosCognitivos.AsNoTracking().Where(x => x.UserToken == token).OrderByDescending(x => x.Fecha).FirstOrDefaultAsync();
        var emocional = await _context.ResultadosEmocionales.AsNoTracking().Where(x => x.UserToken == token).OrderByDescending(x => x.Fecha).FirstOrDefaultAsync();
        var uso = await _context.UsoIA.AsNoTracking().Where(x => x.UserToken == token).OrderByDescending(x => x.Fecha).FirstOrDefaultAsync();
        var sus = await _context.ResultadosSUS.AsNoTracking().Where(x => x.UserToken == token).OrderByDescending(x => x.Fecha).FirstOrDefaultAsync();
        var prediccion = await _context.ResultadosPrediccion.AsNoTracking().Where(x => x.UserToken == token).OrderByDescending(x => x.Fecha).FirstOrDefaultAsync();

        return new ParticipanteDetalleViewModel
        {
            Id = usuario.Id,
            Codigo = CodigoParticipante(usuario.Id),
            FechaRegistro = usuario.FechaRegistro,
            ConsentimientoAceptado = consentimiento?.Aceptado == true,
            FechaConsentimiento = consentimiento?.Fecha,
            LatenciaMs = cognitivo?.LatenciaMs,
            FechaCognitivo = cognitivo?.Fecha,
            Estres = emocional?.Estres,
            Ansiedad = emocional?.Ansiedad,
            Depresion = emocional?.Depresion,
            FechaEmocional = emocional?.Fecha,
            HorasUsoDiario = uso?.HorasUsoDiario,
            DiasSemana = uso?.DiasSemana,
            TipoUso = uso?.TipoUso,
            FechaUsoIA = uso?.Fecha,
            PuntajeSus = sus?.PuntajeTotal,
            FechaSus = sus?.Fecha,
            Nivel = prediccion is null ? "Sin resultado" : NormalizarNivel(prediccion.NivelRiesgo),
            FechaPrediccion = prediccion?.Fecha
        };
    }

    private async Task<IReadOnlyList<ParticipanteResumenViewModel>> CrearResumenesAsync(IReadOnlyList<UsuarioInvestigacion> usuarios)
    {
        if (usuarios.Count == 0) return [];
        var tokens = usuarios.Select(x => x.TokenHash).ToList();

        var consent = (await _context.Consentimientos.AsNoTracking().Where(x => tokens.Contains(x.UserToken) && x.Aceptado).Select(x => x.UserToken).Distinct().ToListAsync()).ToHashSet();
        var cognitivo = (await _context.ResultadosCognitivos.AsNoTracking().Where(x => tokens.Contains(x.UserToken)).Select(x => x.UserToken).Distinct().ToListAsync()).ToHashSet();
        var emocional = (await _context.ResultadosEmocionales.AsNoTracking().Where(x => tokens.Contains(x.UserToken)).Select(x => x.UserToken).Distinct().ToListAsync()).ToHashSet();
        var usoIa = (await _context.UsoIA.AsNoTracking().Where(x => tokens.Contains(x.UserToken)).Select(x => x.UserToken).Distinct().ToListAsync()).ToHashSet();
        var sus = (await _context.ResultadosSUS.AsNoTracking().Where(x => tokens.Contains(x.UserToken)).Select(x => x.UserToken).Distinct().ToListAsync()).ToHashSet();
        var predicciones = await _context.ResultadosPrediccion.AsNoTracking().Where(x => tokens.Contains(x.UserToken)).ToListAsync();
        var ultimas = predicciones.GroupBy(x => x.UserToken).ToDictionary(g => g.Key, g => g.OrderByDescending(x => x.Fecha).First().NivelRiesgo);

        return usuarios.Select(u => new ParticipanteResumenViewModel
        {
            Id = u.Id,
            Codigo = CodigoParticipante(u.Id),
            FechaRegistro = u.FechaRegistro,
            Consentimiento = consent.Contains(u.TokenHash),
            Cognitivo = cognitivo.Contains(u.TokenHash),
            Emocional = emocional.Contains(u.TokenHash),
            UsoIA = usoIa.Contains(u.TokenHash),
            Sus = sus.Contains(u.TokenHash),
            Estado = cognitivo.Contains(u.TokenHash) && emocional.Contains(u.TokenHash) && usoIa.Contains(u.TokenHash) && sus.Contains(u.TokenHash) ? "Completa" : "En progreso",
            Nivel = ultimas.TryGetValue(u.TokenHash, out var nivel) ? NormalizarNivel(nivel) : "Sin resultado"
        }).ToList();
    }

    private static string CodigoParticipante(int id) => $"P-{id:000000}";
    private static string NormalizarNivel(string nivel) => nivel.Equals("ELEVADO", StringComparison.OrdinalIgnoreCase) ? "ALTO" : nivel.ToUpperInvariant();
}
