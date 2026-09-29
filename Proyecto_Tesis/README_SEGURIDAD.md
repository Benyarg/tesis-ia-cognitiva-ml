# Seguridad

## Secretos

Nunca almacenar connection strings, passwords, tokens o claves en Git. Desarrollo usa User Secrets; producción debe usar configuración segura de Azure y preferentemente Managed Identity/Key Vault cuando se despliegue.

## Administración

- Ruta separada `/admin/acceso`.
- No existe registro público.
- Rol `Administrador` obligatorio.
- Contraseña mínima de 12 caracteres, mayúscula, minúscula, número y carácter no alfanumérico.
- Lockout: 5 intentos / 15 minutos.
- Rate limit adicional en login.
- Logout por POST con antiforgery.

## Estudiantes

No crean cuentas. El token de investigación original permanece en la sesión y la base guarda SHA-256. La interfaz administrativa muestra códigos derivados del ID y nunca el token.

## HTTP

CSP, HSTS en producción, anti-clickjacking, nosniff, referrer restringido, Permissions-Policy y cookies Secure/HttpOnly/SameSite.

## Antes de Git

Ejecutar un escaneo de secretos y revisar `git diff --cached` antes del primer commit.
