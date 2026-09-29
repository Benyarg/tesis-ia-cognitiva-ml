# Rendimiento

- Consultas administrativas con `AsNoTracking`.
- Participantes paginados a 25 registros.
- Resúmenes cargados en lotes para evitar N+1 en listados.
- Retry de SQL Server para fallos transitorios.
- Prevención de duplicación de respuestas por doble envío.

Pendiente para grandes volúmenes: índices de base de datos sobre `UserToken` y fechas, caché distribuida si la aplicación escala horizontalmente y persistencia externa de sesión.
