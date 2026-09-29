# Arquitectura

## Separación de superficies

**Público:** `HomeController` + `InvestigacionController`. No requiere Identity. La continuidad se controla mediante una sesión anónima y un token criptográficamente aleatorio cuyo hash se persiste.

**Administración:** `AdminAccountController` + `AdminController`. Requiere ASP.NET Core Identity y el rol `Administrador`.

## Capas

- Presentación: Razor MVC + JS.
- Aplicación: controladores y servicios.
- Persistencia: `ApplicationDbContext` / EF Core.
- Seguridad: Identity, roles, antiforgery, rate limiting, cookies y security headers.
- Analítica: ML.NET y regla de riesgo heredada del MVP.

La estructura permite añadir nuevos roles, servicios de reporting, exportaciones o un modelo ML versionado sin mezclar esas responsabilidades con el flujo público.
