# IA Cognitiva

Plataforma web con Machine Learning orientada a la predicción y
detección temprana de indicadores cognitivos y emocionales en
estudiantes universitarios.

> Los resultados generados por la plataforma son predictivos y de
> detección temprana. No constituyen un diagnóstico clínico ni
> sustituyen una evaluación profesional especializada.

## Estado del proyecto

La aplicación cuenta actualmente con:

- flujo completo de evaluación para participantes;
- consentimiento informado;
- prueba cognitiva;
- evaluación emocional;
- cuestionario de uso de IA;
- evaluación SUS;
- generación de resultados;
- panel administrativo protegido;
- gestión anonimizada de participantes;
- entrenamiento y evaluación del modelo ML;
- ASP.NET Core Identity y autorización por roles;
- protección antiforgery y rate limiting;
- pruebas automatizadas;
- integración continua con GitHub Actions;
- análisis de dependencias con Dependabot;
- despliegue demostrativo en Microsoft Azure.

## Tecnologías

- .NET 10
- ASP.NET Core MVC
- C#
- Entity Framework Core
- ASP.NET Core Identity
- ML.NET
- SQL Server / Azure SQL
- Bootstrap
- JavaScript
- MSTest
- GitHub Actions
- Microsoft Azure

## Entorno demostrativo

El proyecto dispone de una instancia pública destinada exclusivamente
a demostración y pruebas de funcionamiento.

Los datos generados en dicho entorno no forman parte automáticamente
de la muestra oficial de investigación.

## Seguridad

Los secretos y cadenas de conexión no forman parte del repositorio.

En desarrollo se utilizan User Secrets y en producción las
configuraciones se administran desde el entorno de Azure.

## Machine Learning

El sistema permite entrenar y evaluar un modelo utilizando los datos
disponibles en el entorno correspondiente.

Las métricas obtenidas en el entorno demostrativo no representan la
validación experimental definitiva de la investigación.

## Documentación

La documentación técnica adicional se encuentra en `/docs`.

La auditoría inicial se conserva como registro histórico de la
evolución técnica del proyecto.
