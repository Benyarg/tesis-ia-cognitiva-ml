using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Proyecto_Tesis.Data;
using Proyecto_Tesis.ML;
using Proyecto_Tesis.Models;
using Proyecto_Tesis.Models.DTO;
using Proyecto_Tesis.Services;

namespace Proyecto_Tesis.Controllers;

[AllowAnonymous]
[EnableRateLimiting("evaluacion")]
[Route("evaluacion")]
public sealed class InvestigacionController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<InvestigacionController> _logger;

    public InvestigacionController(ApplicationDbContext context, ILogger<InvestigacionController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("cognitiva")]
    public async Task<IActionResult> TestCognitivo() =>
        await MostrarEtapaAsync(RequisitoEtapa.Consentimiento);

    [HttpGet("emocional")]
    public async Task<IActionResult> TestEmocional() =>
        await MostrarEtapaAsync(RequisitoEtapa.Cognitivo);

    [HttpGet("uso-ia")]
    public async Task<IActionResult> UsoIA() =>
        await MostrarEtapaAsync(RequisitoEtapa.Emocional);

    [HttpGet("usabilidad")]
    public async Task<IActionResult> SUS() =>
        await MostrarEtapaAsync(RequisitoEtapa.UsoIA);

    [HttpGet("resultado")]
    public async Task<IActionResult> ResultadoFinal() =>
        await MostrarEtapaAsync(RequisitoEtapa.Sus);

    private async Task<IActionResult> MostrarEtapaAsync(RequisitoEtapa requisito)
    {
        var tokenHash = await ObtenerTokenHashValidoAsync();
        if (tokenHash is null) return RedirectToAction("Index", "Home");
        if (!await CumpleRequisitoAsync(tokenHash, requisito))
            return RedirectToAction("Index", "Home");
        return View();
    }

    [HttpPost("consentimiento")]
    public async Task<IActionResult> GuardarConsentimiento([FromBody] ConsentimientoDto data)
    {
        if (!ModelState.IsValid || !data.Aceptado)
            return BadRequest(new { mensaje = "Debes aceptar el consentimiento para continuar." });

        var tokenSesionExistente = HttpContext.Session.GetString("ResearchToken");
        if (!string.IsNullOrWhiteSpace(tokenSesionExistente))
        {
            var existenteHash = HashToken(tokenSesionExistente);
            if (await _context.Consentimientos.AsNoTracking().AnyAsync(x => x.UserToken == existenteHash && x.Aceptado))
                return Ok(new { mensaje = "El consentimiento ya estaba registrado." });
        }

        var token = GenerarTokenSeguro();
        var tokenHash = HashToken(token);
        var ahora = DateTime.UtcNow;

        _context.UsuariosInvestigacion.Add(new UsuarioInvestigacion
        {
            TokenHash = tokenHash,
            FechaRegistro = ahora
        });
        _context.Consentimientos.Add(new Consentimiento
        {
            UserToken = tokenHash,
            Aceptado = true,
            Fecha = ahora
        });

        await _context.SaveChangesAsync();
        HttpContext.Session.SetString("ResearchToken", token);
        return Ok(new { mensaje = "Consentimiento registrado." });
    }

    [HttpPost("cognitiva")]
    public async Task<IActionResult> GuardarCognitivo([FromBody] LatenciaDto data)
    {
        var token = await ObtenerTokenHashValidoAsync();
        if (token is null) return Unauthorized(new { mensaje = "Sesión no válida." });
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        if (!await CumpleRequisitoAsync(token, RequisitoEtapa.Consentimiento)) return BadRequest(new { mensaje = "El flujo de evaluación no es válido." });

        if (!await _context.ResultadosCognitivos.AnyAsync(x => x.UserToken == token))
        {
            _context.ResultadosCognitivos.Add(new ResultadoCognitivo { UserToken = token, LatenciaMs = data.LatenciaMs, Fecha = DateTime.UtcNow });
            await _context.SaveChangesAsync();
        }
        return Ok(new { mensaje = "Resultado cognitivo guardado." });
    }

    [HttpPost("emocional")]
    public async Task<IActionResult> GuardarEmocional([FromBody] DassDto data)
    {
        var token = await ObtenerTokenHashValidoAsync();
        if (token is null) return Unauthorized(new { mensaje = "Sesión no válida." });
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        if (!await CumpleRequisitoAsync(token, RequisitoEtapa.Cognitivo)) return BadRequest(new { mensaje = "Completa primero la evaluación cognitiva." });

        if (!await _context.ResultadosEmocionales.AnyAsync(x => x.UserToken == token))
        {
            _context.ResultadosEmocionales.Add(new ResultadoEmocional
            {
                UserToken = token, Estres = data.Estres, Ansiedad = data.Ansiedad, Depresion = data.Depresion, Fecha = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
        }
        return Ok(new { mensaje = "Resultado emocional guardado." });
    }

    [HttpPost("uso-ia")]
    public async Task<IActionResult> GuardarUsoIA([FromBody] UsoIADto data)
    {
        var token = await ObtenerTokenHashValidoAsync();
        if (token is null) return Unauthorized(new { mensaje = "Sesión no válida." });
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        if (!await CumpleRequisitoAsync(token, RequisitoEtapa.Emocional)) return BadRequest(new { mensaje = "Completa primero la evaluación emocional." });

        if (!await _context.UsoIA.AnyAsync(x => x.UserToken == token))
        {
            _context.UsoIA.Add(new UsoIA
            {
                UserToken = token, HorasUsoDiario = data.HorasUsoDiario, DiasSemana = data.DiasSemana, TipoUso = data.TipoUso, Fecha = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
        }
        return Ok(new { mensaje = "Uso de IA guardado." });
    }

    [HttpPost("usabilidad")]
    public async Task<IActionResult> GuardarSUS([FromBody] SusDto data)
    {
        var token = await ObtenerTokenHashValidoAsync();
        if (token is null) return Unauthorized(new { mensaje = "Sesión no válida." });
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        if (!await CumpleRequisitoAsync(token, RequisitoEtapa.UsoIA)) return BadRequest(new { mensaje = "Completa primero el cuestionario de uso de IA." });

        if (!await _context.ResultadosSUS.AnyAsync(x => x.UserToken == token))
        {
            _context.ResultadosSUS.Add(new ResultadoSUS { UserToken = token, PuntajeTotal = data.PuntajeTotal, Fecha = DateTime.UtcNow });
            await _context.SaveChangesAsync();
        }
        return Ok(new { mensaje = "Evaluación de usabilidad guardada." });
    }

    [HttpPost("log")]
    public async Task<IActionResult> GuardarLog([FromBody] LogDto data)
    {
        var token = await ObtenerTokenHashValidoAsync();
        if (token is null) return Unauthorized(new { mensaje = "Sesión no válida." });
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        _context.Logs.Add(new InteractionLog
        {
            UserToken = token,
            ActionType = data.ActionType.Trim(),
            LatencyMs = data.LatencyMs,
            InputSize = data.InputSize,
            Timestamp = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("resultado")]
    public async Task<IActionResult> PredecirRiesgo()
    {
        try
        {
            var token = await ObtenerTokenHashValidoAsync();
            if (token is null) return Unauthorized(new { mensaje = "Sesión no válida." });
            if (!await CumpleRequisitoAsync(token, RequisitoEtapa.Sus))
                return BadRequest(new { riesgo = "NO DISPONIBLE", mensaje = "Faltan evaluaciones por completar." });

            var cognitivo = await _context.ResultadosCognitivos.AsNoTracking().Where(x => x.UserToken == token).OrderByDescending(x => x.Fecha).FirstAsync();
            var emocional = await _context.ResultadosEmocionales.AsNoTracking().Where(x => x.UserToken == token).OrderByDescending(x => x.Fecha).FirstAsync();
            var uso = await _context.UsoIA.AsNoTracking().Where(x => x.UserToken == token).OrderByDescending(x => x.Fecha).FirstAsync();

            var fechaEvaluacion = new[] { cognitivo.Fecha, emocional.Fecha, uso.Fecha }.Max();
            var existente = await _context.ResultadosPrediccion.AsNoTracking()
                .Where(x => x.UserToken == token && x.Fecha >= fechaEvaluacion)
                .OrderByDescending(x => x.Fecha)
                .FirstOrDefaultAsync();
            if (existente is not null) return Ok(CrearRespuestaResultado(existente.NivelRiesgo));

            var riesgoRegla = RiskRule.Calcular(cognitivo.LatenciaMs, emocional.Estres, emocional.Ansiedad, emocional.Depresion, uso.HorasUsoDiario, uso.TipoUso);
            var datos = await ObtenerDatosMLAsync();
            var nivelFinal = riesgoRegla ? "ALTO" : "BAJO";

            if (datos.Count >= 10 && datos.Count(x => x.Riesgo) >= 3 && datos.Count(x => !x.Riesgo) >= 3)
            {
                var input = new ModelInput
                {
                    Latencia = (float)cognitivo.LatenciaMs,
                    Estres = emocional.Estres,
                    Ansiedad = emocional.Ansiedad,
                    Depresion = emocional.Depresion,
                    HorasUso = uso.HorasUsoDiario,
                    DiasUso = uso.DiasSemana,
                    TipoUsoReemplazo = uso.TipoUso == "Reemplazo" ? 1f : 0f
                };
                var mlService = new MLService();
                var (modelo, _) = mlService.Entrenar(datos);
                var pred = mlService.Predecir(modelo, input);
                nivelFinal = pred.Prediccion || riesgoRegla ? "ALTO" : "BAJO";
            }

            _context.ResultadosPrediccion.Add(new ResultadoPrediccion
            {
                UserToken = token,
                NivelRiesgo = nivelFinal,
                Probabilidad = 0,
                Fecha = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
            return Ok(CrearRespuestaResultado(nivelFinal));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al generar el resultado predictivo.");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { riesgo = "NO DISPONIBLE", mensaje = "No fue posible generar el resultado en este momento." });
        }
    }

    private async Task<string?> ObtenerTokenHashValidoAsync()
    {
        var rawToken = HttpContext.Session.GetString("ResearchToken");
        if (string.IsNullOrWhiteSpace(rawToken)) return null;
        var tokenHash = HashToken(rawToken);
        return await _context.Consentimientos.AsNoTracking().AnyAsync(x => x.UserToken == tokenHash && x.Aceptado) ? tokenHash : null;
    }

    private async Task<bool> CumpleRequisitoAsync(string token, RequisitoEtapa requisito) => requisito switch
    {
        RequisitoEtapa.Consentimiento => await _context.Consentimientos.AsNoTracking().AnyAsync(x => x.UserToken == token && x.Aceptado),
        RequisitoEtapa.Cognitivo => await _context.ResultadosCognitivos.AsNoTracking().AnyAsync(x => x.UserToken == token),
        RequisitoEtapa.Emocional => await _context.ResultadosEmocionales.AsNoTracking().AnyAsync(x => x.UserToken == token),
        RequisitoEtapa.UsoIA => await _context.UsoIA.AsNoTracking().AnyAsync(x => x.UserToken == token),
        RequisitoEtapa.Sus => await _context.ResultadosSUS.AsNoTracking().AnyAsync(x => x.UserToken == token),
        _ => false
    };

    private async Task<List<ModelInput>> ObtenerDatosMLAsync()
    {
        var cognitivos = await _context.ResultadosCognitivos.AsNoTracking().ToListAsync();
        var emocionales = await _context.ResultadosEmocionales.AsNoTracking().ToListAsync();
        var usos = await _context.UsoIA.AsNoTracking().ToListAsync();
        var ultimosC = cognitivos.GroupBy(x => x.UserToken).ToDictionary(g => g.Key, g => g.OrderByDescending(x => x.Fecha).First());
        var ultimosE = emocionales.GroupBy(x => x.UserToken).ToDictionary(g => g.Key, g => g.OrderByDescending(x => x.Fecha).First());
        var ultimosU = usos.GroupBy(x => x.UserToken).ToDictionary(g => g.Key, g => g.OrderByDescending(x => x.Fecha).First());

        return ultimosC.Keys.Intersect(ultimosE.Keys).Intersect(ultimosU.Keys).Select(token =>
        {
            var c = ultimosC[token]; var e = ultimosE[token]; var u = ultimosU[token];
            return new ModelInput
            {
                Latencia = (float)c.LatenciaMs, Estres = e.Estres, Ansiedad = e.Ansiedad, Depresion = e.Depresion,
                HorasUso = u.HorasUsoDiario, DiasUso = u.DiasSemana, TipoUsoReemplazo = u.TipoUso == "Reemplazo" ? 1f : 0f,
                Riesgo = RiskRule.Calcular(c.LatenciaMs, e.Estres, e.Ansiedad, e.Depresion, u.HorasUsoDiario, u.TipoUso)
            };
        }).ToList();
    }

    private static object CrearRespuestaResultado(string nivel)
    {
        var alto = nivel.Equals("ALTO", StringComparison.OrdinalIgnoreCase) || nivel.Equals("ELEVADO", StringComparison.OrdinalIgnoreCase);
        return new
        {
            riesgo = alto ? "ALTO" : "BAJO",
            mensaje = alto
                ? "La evaluación identifica un nivel alto de indicadores asociados al análisis cognitivo y emocional realizado por la plataforma."
                : "La evaluación identifica un nivel bajo de indicadores asociados al análisis cognitivo y emocional realizado por la plataforma."
        };
    }

    private static string GenerarTokenSeguro() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    private static string HashToken(string token) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));

    private enum RequisitoEtapa { Consentimiento, Cognitivo, Emocional, UsoIA, Sus }
}
