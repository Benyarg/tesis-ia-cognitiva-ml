# Deployment

## Desarrollo

Usar User Secrets para `ConnectionStrings:ConexionSQL`, `AdminSeed:Email` y `AdminSeed:Password`.

## Producción Azure

No copiar User Secrets. Configurar la cadena desde App Service Configuration/Key Vault o migrar a Managed Identity para Azure SQL. HTTPS debe permanecer obligatorio.

Variables esperadas:
- `ConnectionStrings__ConexionSQL`
- `AdminSeed__Email` y `AdminSeed__Password` solo si se necesita crear la cuenta inicial. Retirar la contraseña de seed después de crearla.

Aplicar migraciones de forma controlada antes de iniciar una versión nueva.
