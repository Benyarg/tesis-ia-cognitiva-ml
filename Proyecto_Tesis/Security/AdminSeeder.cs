using Microsoft.AspNetCore.Identity;

namespace Proyecto_Tesis.Security;

public static class AdminSeeder
{
    public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration, ILogger logger)
    {
        using var scope = services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        if (!await roleManager.RoleExistsAsync(AppRoles.Administrador))
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole(AppRoles.Administrador));
            if (!roleResult.Succeeded)
                throw new InvalidOperationException("No se pudo crear el rol Administrador.");
        }

        var email = configuration["AdminSeed:Email"]?.Trim();
        var password = configuration["AdminSeed:Password"];
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            logger.LogInformation("AdminSeed no configurado. No se creará ninguna cuenta administrativa automáticamente.");
            return;
        }

        var existing = await userManager.FindByEmailAsync(email);
        if (existing is null)
        {
            var user = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                LockoutEnabled = true
            };

            var createResult = await userManager.CreateAsync(user, password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join("; ", createResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"No se pudo crear el administrador: {errors}");
            }
            existing = user;
            logger.LogInformation("Cuenta administrativa inicial creada correctamente.");
        }

        if (!await userManager.IsInRoleAsync(existing, AppRoles.Administrador))
        {
            var roleResult = await userManager.AddToRoleAsync(existing, AppRoles.Administrador);
            if (!roleResult.Succeeded)
                throw new InvalidOperationException("No se pudo asignar el rol Administrador.");
        }
    }
}
