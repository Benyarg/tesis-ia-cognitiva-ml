using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Proyecto_Tesis.Security;
using Proyecto_Tesis.ViewModels.Auth;

namespace Proyecto_Tesis.Controllers;

[Route("admin")]
public sealed class AdminAccountController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AdminAccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [AllowAnonymous]
    [HttpGet("acceso")]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true && User.IsInRole(AppRoles.Administrador))
            return RedirectToAction("Index", "Admin");
        return View(new AdminLoginViewModel());
    }

    [AllowAnonymous]
    [EnableRateLimiting("admin-login")]
    [HttpPost("acceso")]
    public async Task<IActionResult> Login(AdminLoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email.Trim());
        if (user is null || !await _userManager.IsInRoleAsync(user, AppRoles.Administrador))
        {
            ModelState.AddModelError(string.Empty, "Credenciales no válidas.");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: true);
        if (result.Succeeded) return RedirectToAction("Index", "Admin");

        ModelState.AddModelError(string.Empty,
            result.IsLockedOut ? "Acceso temporalmente bloqueado por varios intentos fallidos." : "Credenciales no válidas.");
        return View(model);
    }

    [Authorize(Roles = AppRoles.Administrador)]
    [HttpPost("salir")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }

    [AllowAnonymous]
    [HttpGet("denegado")]
    public IActionResult AccessDenied() => View();
}
