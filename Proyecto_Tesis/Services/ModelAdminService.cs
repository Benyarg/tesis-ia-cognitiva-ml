using Microsoft.EntityFrameworkCore;
using Proyecto_Tesis.Data;
using Proyecto_Tesis.ML;
using Proyecto_Tesis.Models;

namespace Proyecto_Tesis.Services;

public interface IModelAdminService
{
    Task<(bool Ok, string Mensaje, ResultadoML? Resultado)> EntrenarAsync();
}

public sealed class ModelAdminService : IModelAdminService
{
    private readonly ApplicationDbContext _context;

    public ModelAdminService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(bool Ok, string Mensaje, ResultadoML? Resultado)> EntrenarAsync()
    {
        var cognitivos = await _context.ResultadosCognitivos.AsNoTracking().ToListAsync();
        var emocionales = await _context.ResultadosEmocionales.AsNoTracking().ToListAsync();
        var usos = await _context.UsoIA.AsNoTracking().ToListAsync();

        var ultimosC = cognitivos.GroupBy(x => x.UserToken).ToDictionary(g => g.Key, g => g.OrderByDescending(x => x.Fecha).First());
        var ultimosE = emocionales.GroupBy(x => x.UserToken).ToDictionary(g => g.Key, g => g.OrderByDescending(x => x.Fecha).First());
        var ultimosU = usos.GroupBy(x => x.UserToken).ToDictionary(g => g.Key, g => g.OrderByDescending(x => x.Fecha).First());

        var datos = ultimosC.Keys.Intersect(ultimosE.Keys).Intersect(ultimosU.Keys).Select(token =>
        {
            var c = ultimosC[token]; var e = ultimosE[token]; var u = ultimosU[token];
            return new ModelInput
            {
                Latencia = (float)c.LatenciaMs,
                Estres = e.Estres,
                Ansiedad = e.Ansiedad,
                Depresion = e.Depresion,
                HorasUso = u.HorasUsoDiario,
                DiasUso = u.DiasSemana,
                TipoUsoReemplazo = u.TipoUso == "Reemplazo" ? 1f : 0f,
                Riesgo = RiskRule.Calcular(c.LatenciaMs, e.Estres, e.Ansiedad, e.Depresion, u.HorasUsoDiario, u.TipoUso)
            };
        }).ToList();

        if (datos.Count < 10)
            return (false, "Se requieren al menos 10 registros completos para entrenar y evaluar el modelo.", null);
        if (datos.Count(x => x.Riesgo) < 3 || datos.Count(x => !x.Riesgo) < 3)
            return (false, "Se requieren al menos tres registros de cada clase para una evaluación mínima.", null);

        var service = new MLService();
        var (_, metricas) = service.Entrenar(datos);
        if (metricas is null)
            return (false, "La partición de prueba no contiene ambas clases. Intenta nuevamente cuando existan más datos.", null);

        var resultado = new ResultadoML
        {
            Accuracy = (float)metricas.Accuracy,
            Precision = (float)metricas.PositivePrecision,
            Recall = (float)metricas.PositiveRecall,
            F1Score = (float)metricas.F1Score,
            Fecha = DateTime.UtcNow
        };

        _context.ResultadosML.Add(resultado);
        await _context.SaveChangesAsync();
        return (
            true,
            "Entrenamiento completado correctamente. Las métricas corresponden a los datos disponibles en el entorno demostrativo y no representan la validación experimental del estudio.",
            resultado
        );
    }
}
