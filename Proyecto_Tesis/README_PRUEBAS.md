# Pruebas

Framework: MSTest.

Incluye pruebas iniciales para:
- regla de riesgo con indicadores bajos/altos;
- comportamiento en umbrales;
- validación de DTO de uso de IA;
- validación del formulario de acceso administrativo.

Ejecutar:

```powershell
dotnet test .\Proyecto_Tesis.Tests\Proyecto_Tesis.Tests.csproj
```

Antes de producción deben añadirse pruebas de integración para autenticación, autorización, antiforgery, flujo completo de evaluación y persistencia contra una base aislada de pruebas.
