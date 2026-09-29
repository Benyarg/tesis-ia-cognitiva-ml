using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Tesis.Security;
using Proyecto_Tesis.Services;

namespace Proyecto_Tesis.Controllers;

[Authorize(Roles = AppRoles.Administrador)]
[Route("admin")]
public sealed class AdminController : Controller
{
    private readonly IAdminDashboardService _dashboard;
    private readonly IModelAdminService _modelAdmin;

    public AdminController(IAdminDashboardService dashboard, IModelAdminService modelAdmin)
    {
        _dashboard = dashboard;
        _modelAdmin = modelAdmin;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index() => View(await _dashboard.ObtenerDashboardAsync());

    [HttpGet("participantes")]
    public async Task<IActionResult> Participantes([FromQuery] string? q, [FromQuery] int page = 1) => View(await _dashboard.ObtenerParticipantesAsync(q, page));

    [HttpGet("participantes/{id:int}")]
    public async Task<IActionResult> Detalle(int id)
    {
        var model = await _dashboard.ObtenerDetalleAsync(id);
        return model is null ? NotFound() : View(model);
    }

    [HttpPost("modelo/entrenar")]
    public async Task<IActionResult> EntrenarModelo()
    {
        var resultado = await _modelAdmin.EntrenarAsync();
        TempData[resultado.Ok ? "Success" : "Error"] = resultado.Mensaje;
        return RedirectToAction(nameof(Index));
    }
}
