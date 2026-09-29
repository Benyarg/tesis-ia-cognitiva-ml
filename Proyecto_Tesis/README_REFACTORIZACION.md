# Refactorización

- Separación de autenticación administrativa del flujo público.
- Eliminación del UI público de Identity y de registro de usuarios.
- Servicios administrativos separados del controlador.
- Regla de riesgo centralizada en `RiskRule`.
- ViewModels específicos para dashboard y autenticación.
- Rutas semánticas `/evaluacion/*` y `/admin/*`.
- Paginación administrativa para evitar cargar todos los participantes.
- Eliminación de dependencias NuGet no utilizadas.
