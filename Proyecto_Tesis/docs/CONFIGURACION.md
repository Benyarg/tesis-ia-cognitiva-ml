# Configuración

## Requisitos

- .NET 10 SDK
- SQL Server o Azure SQL
- Visual Studio 2022 o equivalente

## Base de datos

La cadena de conexión no se almacena en el repositorio.

Para desarrollo:

```powershell
dotnet user-secrets set "ConnectionStrings:ConexionSQL" "TU_CADENA_DE_CONEXION"