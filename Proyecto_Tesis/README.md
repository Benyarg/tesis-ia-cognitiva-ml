# IA Cognitiva — Proyecto de Tesis

**Plataforma web con Machine Learning para la predicción y detección temprana del deterioro cognitivo y emocional en estudiantes universitarios de Cajamarca – 2026.**

## Alcance final

La aplicación tiene dos superficies separadas:

- **Estudiante:** acceso público y anónimo. Acepta el consentimiento, completa las evaluaciones cognitiva, emocional, uso de IA y SUS, y visualiza únicamente un resultado final de **nivel alto** o **nivel bajo**. No existe login ni dashboard para estudiantes.
- **Administrador:** acceso privado en `/admin/acceso`, protegido por ASP.NET Core Identity y rol `Administrador`. Permite revisar participantes anonimizados, progreso, resultados, detalle de evaluaciones y métricas registradas del modelo.

Los resultados son predictivos y de detección temprana; **no constituyen diagnósticos clínicos**.

## Tecnologías

- ASP.NET Core MVC / .NET 10
- Entity Framework Core 10 + SQL Server / Azure SQL
- ASP.NET Core Identity + roles
- ML.NET
- Bootstrap 5 (local)
- JavaScript vanilla
- MSTest

## Seguridad aplicada

- Sin credenciales en el repositorio.
- `.NET User Secrets` para desarrollo.
- Cuenta administrativa creada desde secretos de configuración.
- Rol `Administrador` obligatorio para `/admin/*`.
- Sin registro/login público de estudiantes.
- Cookies `HttpOnly`, `Secure` y `SameSite=Strict`.
- Antiforgery/CSRF global.
- Rate limiting en login administrativo y endpoints de evaluación.
- Bloqueo de cuenta tras intentos fallidos.
- Cabeceras CSP, HSTS, `X-Content-Type-Options`, `X-Frame-Options`, `Referrer-Policy` y `Permissions-Policy`.
- Tokens de investigación aleatorios; en base de datos se almacena su hash SHA-256.
- Mensajes de error sin detalles internos.
- Datos administrativos mostrados mediante códigos anonimizados `P-000001`.

## Configuración inicial

Desde `Proyecto_Tesis/Proyecto_Tesis`:

```powershell
dotnet user-secrets set "ConnectionStrings:ConexionSQL" "TU_CADENA_DE_CONEXION"
dotnet user-secrets set "AdminSeed:Email" "TU_CORREO_ADMIN"
dotnet user-secrets set "AdminSeed:Password" "TU_PASSWORD_ADMIN_SEGURA"
```

Después:

```powershell
dotnet tool restore
dotnet restore
dotnet ef database update
dotnet build
dotnet test ..\Proyecto_Tesis.Tests\Proyecto_Tesis.Tests.csproj
dotnet run
```

Al iniciar por primera vez, el sistema crea el rol `Administrador` y la cuenta configurada. Luego puedes retirar del almacén local la contraseña de seed:

```powershell
dotnet user-secrets remove "AdminSeed:Password"
```

La cuenta ya creada permanece en ASP.NET Core Identity.

## Endpoints principales

### Público / estudiante

- `/` — consentimiento
- `/evaluacion/cognitiva`
- `/evaluacion/emocional`
- `/evaluacion/uso-ia`
- `/evaluacion/usabilidad`
- `/evaluacion/resultado`

### Administración

- `/admin/acceso`
- `/admin`
- `/admin/participantes`
- `/admin/participantes/{id}`
- `POST /admin/modelo/entrenar`

## Machine Learning

El proyecto conserva la lógica del MVP y permite entrenar/evaluar ML.NET desde el área administrativa cuando existen datos mínimos. Los umbrales de la regla de riesgo heredada del MVP deben conservar trazabilidad con la metodología de tesis. Las métricas mostradas corresponden a los datos disponibles y no deben generalizarse sin validación metodológica.

## Estructura

- `Controllers/` — flujo público y administración.
- `Data/` — EF Core / Identity.
- `Models/` — entidades y DTOs.
- `Services/` — dashboard, regla de riesgo y entrenamiento administrativo.
- `Security/` — roles y seed seguro de administrador.
- `ViewModels/` — modelos específicos de UI.
- `Views/` — interfaces pública y privada.
- `ML/` — servicio ML.NET.
- `Proyecto_Tesis.Tests/` — pruebas MSTest.

## Estado

Versión final de implementación preparada para validación local, pruebas con Azure SQL y posterior creación de un repositorio Git nuevo. No incluye `.git`, secretos, `bin/` ni `obj/`.
