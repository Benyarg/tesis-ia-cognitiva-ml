# Seguridad

La aplicación implementa controles de seguridad para proteger el acceso administrativo y los datos procesados dentro de la plataforma.

## Controles principales

- ASP.NET Core Identity para autenticación.
- Autorización mediante rol `Administrador`.
- Ruta administrativa separada del flujo público.
- Protección antiforgery en formularios y operaciones sensibles.
- Rate limiting en accesos y operaciones críticas.
- Bloqueo temporal después de múltiples intentos fallidos de inicio de sesión.
- Cookies configuradas con `Secure`, `HttpOnly` y `SameSite`.
- HTTPS y HSTS en producción.
- Content Security Policy y encabezados HTTP de seguridad.
- Sin registro público de administradores.

## Gestión de secretos

Las credenciales y cadenas de conexión no se almacenan en el repositorio.

- Desarrollo: .NET User Secrets.
- Producción: configuración segura mediante Azure App Service.

Nunca deben subirse a Git:

- contraseñas;
- cadenas de conexión;
- tokens;
- claves privadas;
- secretos de servicios externos;
- perfiles de publicación con credenciales.

## Participantes

Los participantes no requieren una cuenta de usuario.

La plataforma utiliza identificadores pseudónimos para relacionar las evaluaciones sin mostrar información identificable en el panel administrativo.

## Administración

El panel administrativo se encuentra protegido mediante autenticación y autorización por rol.

El acceso se realiza desde:

```text
/admin/acceso