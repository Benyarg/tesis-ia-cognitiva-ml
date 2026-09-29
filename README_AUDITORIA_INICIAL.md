# README_AUDITORIA_INICIAL.md

## Proyecto de tesis

**Plataforma web con Machine Learning para la predicción y detección temprana del deterioro cognitivo y emocional en estudiantes universitarios de Cajamarca – 2026.**

## 1. Objetivo de esta auditoría

Evaluar el estado real del MVP antes de modificar código, identificando arquitectura, módulos, funcionalidades, deuda técnica, bugs, riesgos metodológicos, problemas de seguridad, problemas de UI/UX, estado del flujo de evaluación y estado actual del componente de Machine Learning.

Esta fase **no modifica la implementación**. Su finalidad es establecer una línea base técnica y priorizar los cambios posteriores.

> Principio funcional: la plataforma debe apoyar la **predicción y detección temprana de indicadores** relacionados con el deterioro cognitivo y emocional. No debe presentarse como una herramienta de diagnóstico clínico.

---

# 2. Resumen ejecutivo

El proyecto contiene un MVP reconocible y con varias piezas importantes ya implementadas: ASP.NET Core MVC/Razor, Entity Framework Core, Identity, SQL Server, sesiones, consentimiento, prueba cognitiva, DASS-21, cuestionario de uso de IA, SUS, registro de interacción y un flujo de predicción con ML.NET.

Sin embargo, **el estado actual no es adecuado todavía para volver a publicarlo en GitHub ni para usarlo como versión de sustentación o portafolio**. Existen bloqueadores críticos de seguridad, funcionamiento e integridad metodológica.

### Estado global

| Área | Estado | Severidad dominante |
|---|---|---|
| Seguridad de secretos | Bloqueada | **CRÍTICO** |
| Flujo funcional actual | Inestable | **CRÍTICO / ALTO** |
| Sesiones | Configuración incompleta | **ALTO** |
| Autenticación/autorización | Parcial y desconectada del flujo de investigación | **ALTO** |
| Formularios/validación | MVP, validación principalmente cliente | **ALTO** |
| Machine Learning | Funcional como prototipo, metodológicamente no consolidado | **CRÍTICO / ALTO** |
| Resultados | Incompatibles en parte con el enfoque no diagnóstico | **ALTO** |
| Dashboard estudiante | No implementado | **MEDIO** |
| Dashboard administrador | No implementado | **ALTO** por requerimientos de autorización futuros |
| Pruebas automatizadas | No encontradas | **ALTO** |
| Responsive | Parcial | **MEDIO** |
| Accesibilidad | Parcial | **MEDIO** |
| Documentación | Prácticamente ausente | **MEDIO** |
| Preparación para deployment | No segura todavía | **CRÍTICO** |

### Decisión de auditoría

**No se recomienda avanzar todavía a rediseño visual, dashboard o deployment.**

Primero deben resolverse, en este orden:

1. credenciales expuestas y política de secretos;
2. saneamiento del repositorio Git;
3. errores que rompen el flujo de CSS/JavaScript;
4. configuración de sesión y protección de endpoints;
5. validación server-side;
6. corrección del flujo de predicción/entrenamiento;
7. revisión metodológica de la etiqueta y métricas del modelo.

---

# 3. Alcance inspeccionado

Se revisaron los siguientes elementos del ZIP entregado:

- solución `.slnx`;
- proyecto ASP.NET Core;
- historial Git incluido en `.git`;
- `Program.cs`;
- `appsettings.json` y `appsettings.Development.json`;
- controladores;
- `ApplicationDbContext`;
- entidades y DTO;
- migraciones EF Core;
- páginas de ASP.NET Core Identity;
- vistas Razor;
- JavaScript del flujo de investigación;
- hojas de estilo;
- configuración de publicación de Azure;
- archivos de dependencias de servicio;
- componente ML.NET;
- archivo `MLModel.mlnet`;
- estado de pruebas y documentación.

### Limitación de esta auditoría

No se pudo ejecutar `dotnet build`, `dotnet test` ni un análisis NuGet de vulnerabilidades porque el entorno de auditoría no dispone del SDK `dotnet`. Por ello, los hallazgos de compilación/runtime se basan en inspección estática y deben verificarse posteriormente en un entorno con .NET instalado.

---

# 4. Arquitectura encontrada

## 4.1 Estilo general

Actualmente el proyecto corresponde a un **monolito ASP.NET Core MVC/Razor**.

```text
Navegador
   |
   | HTTP / Fetch
   v
ASP.NET Core MVC + Razor Pages
   |
   |-- Controllers
   |-- ASP.NET Core Identity
   |-- Session
   |-- Entity Framework Core
   |-- MLService (ML.NET)
   |
   v
SQL Server
```

No existe una API independiente, frontend SPA separado ni capa de aplicación formal.

## 4.2 Capas reales observadas

### Presentación

- `Views/Home`
- `Views/Investigacion`
- `Views/Shared`
- `Areas/Identity`
- `wwwroot/css`
- `wwwroot/js`

### Controladores

- `HomeController`
- `InvestigacionController`

### Persistencia

- `ApplicationDbContext`
- EF Core
- SQL Server
- Migrations

### Dominio/datos

- modelos EF en `Models`
- DTO simples en `Models/DTO`

### Machine Learning

- `ML/MLService.cs`
- `ML/ModelInput.cs`
- `ML/ModelOuput.cs`
- `MLModel.mlnet`

## 4.3 Observación de arquitectura

`InvestigacionController` concentra demasiadas responsabilidades:

- navegación;
- consentimiento;
- identificación mediante sesión;
- escritura de resultados;
- logging de interacción;
- preparación de dataset;
- etiquetado de riesgo;
- entrenamiento;
- evaluación del modelo;
- predicción;
- persistencia de métricas;
- construcción del mensaje final.

Esto convierte al controlador en un punto de alta complejidad y dificulta pruebas, seguridad y mantenimiento.

**Severidad: ALTO.**

---

# 5. Inconsistencia de versión tecnológica

La especificación proporcionada solicita buenas prácticas de **C# con .NET 9.0**.

El proyecto inspeccionado contiene:

```xml
<TargetFramework>net10.0</TargetFramework>
```

Además, los paquetes principales de ASP.NET Core / EF Core se encuentran en versión `10.0.6`.

### Impacto

No debe realizarse una migración automática ni degradar el proyecto sin decisión previa. Es necesario definir una sola versión objetivo y alinear:

- memoria de tesis;
- documentación técnica;
- entorno de desarrollo;
- Azure;
- CI/CD;
- dependencias;
- screenshots y portafolio.

**Severidad: MEDIO.**

---

# 6. Funcionalidades existentes

## 6.1 Flujo de participante

Se identificó el siguiente flujo pretendido:

```text
Inicio / consentimiento
   -> Test cognitivo
   -> DASS-21
   -> Uso de IA
   -> SUS
   -> Resultado final
```

## 6.2 Funcionalidades implementadas

### Consentimiento

Existe una pantalla de consentimiento en `Views/Home/Index.cshtml` y un endpoint `GuardarConsentimiento`.

### Test cognitivo

Existe una prueba de reacción de tres intentos. Se calcula la latencia promedio y se envía al backend.

### Evaluación emocional

Existe un cuestionario DASS-21 cargado mediante JavaScript y se calculan tres puntajes:

- estrés;
- ansiedad;
- depresión.

### Cuestionario de uso de IA

Se registran:

- horas de uso diario;
- días por semana;
- tipo de uso.

### SUS

Existe un cuestionario System Usability Scale y cálculo del puntaje total.

### Logs de interacción

Existe almacenamiento de:

- tipo de acción;
- latencia;
- tamaño de entrada;
- timestamp.

### Predicción

Existe lógica para:

- preparar datos;
- generar una etiqueta de riesgo mediante una regla;
- entrenar un clasificador ML.NET;
- realizar predicción;
- combinar regla + ML;
- persistir resultados.

### Identity

Existe scaffolding de:

- registro;
- login;
- tablas de Identity.

Sin embargo, este sistema de cuentas no está correctamente integrado con el flujo pseudónimo de investigación.

---

# 7. Funcionalidades incompletas o ausentes

No se encontraron implementados de forma funcional:

- dashboard del estudiante;
- dashboard administrativo;
- gestión real de roles;
- autorización administrativa;
- historial visual del estudiante;
- filtros/búsqueda de participantes;
- gestión administrativa de evaluaciones;
- visualización agregada de indicadores;
- versionado formal del modelo ML;
- pipeline persistente de entrenamiento;
- modelo cargado desde almacenamiento para inferencia;
- progreso persistente entre pasos;
- reanudación segura de una evaluación;
- prevención de doble envío;
- sistema de estados de evaluación;
- auditoría administrativa;
- tests automatizados;
- documentación técnica;
- documentación de deployment;
- política de secretos;
- CI de seguridad;
- secret scanning previo a push;
- Content Security Policy;
- manejo centralizado de errores.

---

# 8. Hallazgos CRÍTICOS

## C-01 — Credenciales de Azure SQL presentes en Git

**Archivo histórico:** `Proyecto_Tesis/appsettings.json` dentro del commit actual incluido en `.git`.

El historial contiene una entrada `ConexionSQLAzure` con:

- host Azure SQL;
- usuario;
- contraseña.

Los valores no se reproducen en este informe.

La copia de trabajo actual ya eliminó `ConexionSQLAzure`, pero el secreto **sigue existiendo en el objeto Git versionado**.

### Impacto

Una persona que haya obtenido el repositorio o el commit histórico podría recuperar esas credenciales mientras sigan siendo válidas.

### Estado

**CRÍTICO. BLOQUEA PUBLICACIÓN.**

### Acción requerida en la fase de seguridad

1. Confirmar revocación/rotación real de las credenciales en Azure.
2. Considerar comprometidas las credenciales que estuvieron publicadas.
3. Crear un historial limpio antes de publicar un nuevo repositorio.
4. No volver a usar esa contraseña.
5. Mover secretos de desarrollo a User Secrets.
6. Mover secretos de producción a configuración segura de Azure/Key Vault.
7. Añadir protección preventiva de secretos en Git.

---

## C-02 — `.gitignore` no protege `appsettings*.json`

El `.gitignore` actual ignora principalmente:

- `.vs/`;
- `bin/`;
- `obj/`;
- `*.user`;
- logs;
- paquetes.

No protege explícitamente:

- `appsettings.json`;
- `appsettings.Development.json`;
- variantes locales de configuración;
- archivos `.env`;
- potenciales archivos de secretos.

Además, `appsettings.json` y `appsettings.Development.json` están actualmente rastreados por Git.

### Impacto

Alta probabilidad de repetir el incidente de exposición de credenciales.

**Severidad: CRÍTICO.**

> En la fase de seguridad se deberá conservar un `appsettings.json` seguro únicamente con configuración no secreta o placeholders y almacenar los secretos fuera del repositorio.

---

## C-03 — El flujo JavaScript principal puede no cargar

Varias vistas contienen construcciones similares a:

```html
<link href="~/js/..." rel="script" />
```

Un archivo JavaScript debe cargarse mediante `<script src="..."></script>` o una sección Razor equivalente.

Afecta, entre otros:

- `Views/Home/Index.cshtml`;
- `Views/Investigacion/TestCognitivo.cshtml`;
- `Views/Investigacion/TestEmocional.cshtml`;
- `Views/Investigacion/UsoIA.cshtml`;
- `Views/Investigacion/SUS.cshtml`;
- `Views/Investigacion/ResultadoFinal.cshtml`.

### Impacto

Funciones como:

- `iniciar()`;
- `iniciarTest()`;
- envío DASS-21;
- envío de uso IA;
- envío SUS;
- `obtenerResultado()`;

pueden no estar disponibles en navegador.

**Severidad: CRÍTICO funcional.**

---

## C-04 — Rutas de CSS inconsistentes

Las vistas referencian rutas como:

```text
/css/style-index.css
/css/investigacion-views/...
```

pero los archivos reales están ubicados en:

```text
/css/home/style-index.css
/css/views-investigacion/...
```

### Impacto

- estilos no cargados;
- interfaz inconsistente;
- falsa impresión de fallos responsive;
- dificultad para evaluar UI/UX real.

**Severidad: CRÍTICO funcional para la versión actual.**

---

## C-05 — Riesgo metodológico del modelo ML

En `InvestigacionController.CalcularRiesgo(...)` se construye la etiqueta `Riesgo` usando exactamente variables que luego se entregan al modelo como características:

- latencia;
- estrés;
- ansiedad;
- depresión;
- horas de uso;
- tipo de uso.

Después el modelo aprende esa etiqueta generada internamente.

### Problema

Esto puede hacer que el modelo aprenda principalmente una **regla creada por el propio sistema**, en lugar de aprender un resultado objetivo independiente de investigación.

Las métricas obtenidas de esta etiqueta no deben interpretarse automáticamente como evidencia de capacidad real de detección temprana en estudiantes.

### Impacto académico

- riesgo de circularidad;
- métricas potencialmente optimistas;
- dificultad para justificar validez predictiva;
- posible inconsistencia entre metodología, variable objetivo y modelo.

**Severidad: CRÍTICO metodológico.**

### Condición antes de modificar

Debe verificarse contra:

- metodología de tesis;
- variable dependiente;
- instrumento/criterio de etiqueta;
- dataset real;
- protocolo de validación.

No se debe “corregir” automáticamente desde código sin validar primero esa correspondencia académica.

---

# 9. Hallazgos de severidad ALTA

## A-01 — Session configurada sin backing store visible

`Program.cs` contiene:

```csharp
builder.Services.AddSession();
...
app.UseSession();
```

pero no se observa configuración explícita de un `IDistributedCache`, por ejemplo `AddDistributedMemoryCache`, Redis o equivalente.

### Impacto

La sesión es fundamental porque `UserToken` controla el acceso al flujo. Una configuración incompleta puede provocar fallos o comportamiento impredecible de sesión.

**Severidad: ALTO.**

---

## A-02 — Autorización incompleta

No se encontraron atributos `[Authorize]` en los controladores de investigación.

La protección actual se basa en una comprobación manual de sesión en determinadas vistas:

```csharp
UsuarioValido()
```

pero no se aplica de manera uniforme a todos los endpoints de escritura y ML.

### Ejemplo crítico

`EntrenarModelo()` no contiene control de rol ni autenticación administrativa.

### Impacto

Un endpoint sensible de entrenamiento podría quedar accesible sin una frontera administrativa real.

**Severidad: ALTO.**

---

## A-03 — Endpoint de entrenamiento con efecto lateral mediante GET implícito

`EntrenarModelo()` no especifica `[HttpPost]` y persiste métricas en la base de datos.

### Problema

Una operación que modifica estado no debe depender de una solicitud GET convencional.

### Impacto

- activación accidental;
- activación por crawler/preload;
- repetición de entrenamiento;
- nuevas filas de métricas sin control;
- superficie administrativa expuesta.

**Severidad: ALTO.**

---

## A-04 — `PredecirRiesgo` usa GET y escribe en base de datos

`PredecirRiesgo()` está marcado `[HttpGet]`, pero ejecuta:

```csharp
_context.ResultadosPrediccion.Add(resultado);
_context.SaveChanges();
```

### Impacto

Cada actualización de la pantalla o nueva llamada puede crear otra fila de predicción.

Esto puede generar:

- duplicados;
- historial artificial;
- métricas administrativas incorrectas;
- dificultad para saber qué resultado corresponde a una evaluación real.

**Severidad: ALTO.**

---

## A-05 — El modelo se vuelve a entrenar durante la predicción

En `PredecirRiesgo()` se ejecuta de nuevo:

```csharp
var (modelo, _) = mlService.Entrenar(datos);
```

### Impacto

- latencia innecesaria;
- resultado no determinista por cada solicitud;
- modelo distinto entre usuarios/requests;
- imposibilidad de versionar correctamente el modelo;
- dificultad para reproducir resultados de tesis;
- pérdida de trazabilidad científica.

**Severidad: ALTO.**

---

## A-06 — Riesgo de duplicación del dataset por joins

`ObtenerDatosML()` une tablas mediante `UserToken` sin seleccionar explícitamente una evaluación o registro emparejado.

Si un participante posee múltiples resultados cognitivos, emocionales o de uso IA, el join puede producir múltiples combinaciones para el mismo participante.

### Impacto

- duplicación de filas;
- distorsión del dataset;
- fuga entre train/test;
- métricas no representativas.

**Severidad: ALTO.**

---

## A-07 — Falta de validación server-side en DTO

DTO como:

- `DassDto`;
- `LatenciaDto`;
- `UsoIADto`;
- `LogDto`;
- `SusDto`;

no contienen reglas de validación.

El navegador limita algunos campos, pero un cliente puede enviar directamente valores como:

- horas negativas;
- días fuera de 1–7;
- puntajes emocionales fuera del rango;
- strings inesperados;
- latencias negativas o extremas;
- puntajes SUS inválidos.

### Impacto

Los datos de investigación pueden corromperse sin que el sistema lo detecte.

**Severidad: ALTO.**

---

## A-08 — Los fetch no validan `response.ok`

Ejemplo general:

```javascript
fetch(...)
    .then(() => {
        window.location.href = '...';
    });
```

Una respuesta HTTP 400/500 sigue resolviendo la promesa `fetch`; por tanto, el frontend puede continuar al siguiente paso aunque el backend no haya guardado correctamente el dato.

### Impacto

- evaluaciones incompletas;
- datos faltantes;
- usuario cree que todo fue registrado;
- predicción final sin datos completos.

**Severidad: ALTO.**

---

## A-09 — Guardado cognitivo no esperado antes de continuar

`enviarResultado(promedio)` dispara un `fetch`, pero el flujo no espera su confirmación antes de programar la navegación a la siguiente evaluación.

### Impacto

En conexiones lentas o fallos de red, la navegación puede ocurrir sin que el resultado quede persistido.

**Severidad: ALTO.**

---

## A-10 — Falta de protección antiforgery visible en endpoints propios

Los POST personalizados no muestran `[ValidateAntiForgeryToken]` ni una estrategia equivalente para las llamadas AJAX.

### Impacto

Debe revisarse la protección CSRF antes de deployment, especialmente porque la aplicación usa cookies/sesión y operaciones de escritura.

**Severidad: ALTO.**

---

## A-11 — El token de investigación se devuelve al cliente y se registra en consola

`GuardarConsentimiento()` devuelve el token y `home.js` ejecuta:

```javascript
console.log("Token:", token);
```

### Impacto

El identificador pseudónimo utilizado para enlazar datos de investigación queda innecesariamente expuesto en:

- respuesta HTTP;
- DevTools;
- posibles registros de depuración.

**Severidad: ALTO por privacidad.**

---

## A-12 — Dos mecanismos de identidad no integrados

Existen simultáneamente:

1. ASP.NET Core Identity;
2. token pseudónimo guardado en Session.

No existe una relación clara entre ambos.

Además:

- `CrearUsuario()` agrega `UsuarioInvestigacion`;
- el flujo normal llama `GuardarConsentimiento()`;
- `GuardarConsentimiento()` no crea `UsuarioInvestigacion`.

### Impacto

- datos huérfanos conceptualmente;
- tabla `UsuariosInvestigacion` puede no representar a quienes completan el estudio;
- arquitectura de identidad difícil de explicar en sustentación.

**Severidad: ALTO.**

---

## A-13 — Política de Identity debilitada para pruebas

`Program.cs` desactiva requisitos de contraseña:

- dígito;
- carácter no alfanumérico;
- mayúscula;
- longitud mínima fuerte;

Además:

```csharp
options.SignIn.RequireConfirmedAccount = false;
```

El login usa:

```csharp
lockoutOnFailure: false
```

### Impacto

Si Identity se utiliza posteriormente para administradores, esta configuración no es adecuada como configuración de producción.

**Severidad: ALTO si las cuentas quedan expuestas en producción.**

---

## A-14 — Exposición de excepciones al cliente

En `PredecirRiesgo()`:

```csharp
mensaje = "Error interno: " + ex.Message
```

### Impacto

Una excepción puede revelar:

- información de base de datos;
- estructura interna;
- nombres de objetos;
- detalles útiles para reconocimiento técnico.

**Severidad: ALTO.**

---

## A-15 — Terminología diagnóstica en la UI

`Views/Investigacion/ResultadoFinal.cshtml` contiene:

> “generar el diagnóstico”

Esto contradice el principio de la tesis de predicción/detección temprana y no diagnóstico.

También existe un mensaje de riesgo bajo:

> “Tu estado actual se encuentra dentro de parámetros adecuados.”

Este lenguaje puede interpretarse como una afirmación clínica más fuerte que lo que el sistema realmente puede sostener.

**Severidad: ALTO académico/ético.**

---

## A-16 — No existen pruebas automatizadas

No se encontró proyecto MSTest ni archivos de test propios.

### Impacto

No existe una red de seguridad para:

- reglas de negocio;
- cálculo DASS;
- cálculo SUS;
- token/sesión;
- guardados;
- predicción;
- autorización;
- validaciones;
- casos límite.

**Severidad: ALTO.**

---

# 10. Hallazgos de severidad MEDIA

## M-01 — Archivo ML entrenado aparentemente no integrado

Existe:

```text
Proyecto_Tesis/MLModel.mlnet
```

pero el `.csproj` intenta incluir:

```text
Models\MLModel.mlnet
```

solo si existe esa ruta.

No se encontró código que cargue `MLModel.mlnet` para inferencia.

### Impacto

No queda claro cuál es la fuente de verdad del modelo:

- archivo `.mlnet`;
- entrenamiento dinámico;
- regla `CalcularRiesgo`.

**Severidad: MEDIO/ALTO metodológico.**

---

## M-02 — Entrenamiento no reproducible

`TrainTestSplit` no utiliza una semilla explícita y el `MLContext` tampoco fija seed.

### Impacto

Las métricas pueden variar entre ejecuciones, dificultando reproducibilidad académica.

**Severidad: MEDIO.**

---

## M-03 — Umbrales de riesgo hardcodeados sin trazabilidad documental

La función `CalcularRiesgo` contiene umbrales como:

- latencia > 500 / 700;
- estrés > 10 / 15;
- ansiedad > 10 / 15;
- depresión > 10 / 15;
- horas de IA >= 5;
- tipo `Reemplazo`.

El repositorio no contiene documentación que explique la procedencia científica de esos umbrales ni su correspondencia exacta con los instrumentos.

### Acción

No modificar todavía. Primero verificar metodología/instrumentos.

**Severidad: MEDIO/ALTO académico.**

---

## M-04 — DASS-21 calculado en frontend

La asignación de ítems y suma se realiza en JavaScript.

### Impacto

Un usuario puede modificar el payload antes de enviarlo. Para integridad de investigación, el backend debería validar o recalcular lo necesario a partir de datos definidos por el protocolo.

**Severidad: MEDIO/ALTO.**

---

## M-05 — SUS calculado en frontend

El puntaje SUS se calcula completamente en navegador y el backend recibe únicamente `PuntajeTotal`.

### Impacto

No existe evidencia server-side de las respuestas originales ni posibilidad de auditar el cálculo.

**Severidad: MEDIO.**

---

## M-06 — No hay relaciones FK entre tablas de investigación

Los datos se relacionan mediante `UserToken` string, pero no se observaron claves foráneas ni índices de dominio para esas entidades.

### Impacto

- integridad referencial débil;
- posibilidad de registros huérfanos;
- consultas más costosas;
- dificultad para modelar una “evaluación” como unidad coherente.

**Severidad: MEDIO.**

---

## M-07 — No existe entidad de Evaluación/Sesión de evaluación

Actualmente cada tabla guarda resultados independientemente por `UserToken`.

No existe un identificador que agrupe de manera inequívoca:

```text
una ejecución cognitiva
+ una ejecución emocional
+ un cuestionario de uso IA
+ un SUS
+ una predicción
```

### Impacto

Al permitir repetir evaluaciones, será difícil saber qué registros pertenecen al mismo ciclo.

**Severidad: MEDIO/ALTO.**

---

## M-08 — Uso extensivo de operaciones síncronas EF

Se usan repetidamente:

```csharp
SaveChanges();
ToList();
FirstOrDefault();
Any();
```

sin variantes async.

En un MVP pequeño puede funcionar, pero para deployment y concurrencia conviene revisar.

**Severidad: MEDIO.**

---

## M-09 — Recursos externos sin política CSP visible

`_Layout.cshtml` carga recursos desde:

- jsDelivr;
- Google Fonts.

No se observó Content Security Policy.

### Impacto

Para una plataforma de investigación conviene reducir dependencias externas, documentar privacidad y endurecer políticas de contenido.

**Severidad: MEDIO.**

---

## M-10 — Potencial sink XSS mediante `innerHTML`

`resultado-final.js` inserta `data.mensaje` mediante `innerHTML`.

Actualmente los mensajes están definidos en backend, pero la práctica crea un riesgo futuro si ese dato llega a incorporar contenido controlado externamente.

**Severidad: MEDIO.**

---

## M-11 — `DateTime.Now` en persistencia

Se usa la hora local del servidor en múltiples entidades.

### Riesgo

Cambios de zona horaria o deployment pueden dificultar trazabilidad. Para auditoría y análisis suele ser preferible una estrategia consistente basada en UTC y conversión solo en presentación.

**Severidad: MEDIO.**

---

## M-12 — No existe prevención de doble envío

Los botones no se deshabilitan durante guardado y no existe un identificador idempotente de operación.

### Impacto

Doble clic o reintentos pueden crear registros repetidos.

**Severidad: MEDIO/ALTO.**

---

## M-13 — Responsive aún superficial

Las hojas de estilo utilizan principalmente `max-width`, pero casi no contienen breakpoints específicos para las pantallas objetivo definidas en el proyecto.

No existe evidencia de pruebas sistemáticas en:

- 360;
- 390;
- 430;
- 768;
- 1024;
- 1280;
- 1366;
- 1440;
- 1920 px.

**Severidad: MEDIO.**

---

## M-14 — Accesibilidad parcial

Aspectos positivos:

- algunos labels están correctamente asociados;
- formularios de Identity usan validación y roles de alerta.

Pendientes:

- interacción principal mediante `onclick` inline;
- feedback mediante `alert()`;
- prueba cognitiva depende fuertemente del color verde;
- falta de estados ARIA en resultados/progreso;
- no hay evidencia de navegación completa por teclado;
- no hay evaluación de contraste formal;
- falta de focus management entre pasos.

**Severidad: MEDIO.**

---

# 11. Hallazgos de severidad BAJA

## B-01 — Código duplicado/dead code en `HomeController`

`HomeController` contiene acciones de pruebas que duplican rutas conceptualmente existentes en `InvestigacionController`, aunque las vistas correspondientes se encuentran bajo `Views/Investigacion`.

Esto sugiere restos del MVP.

**Severidad: BAJO.**

## B-02 — Nombre `ModelOuput.cs`

Existe un typo en el nombre físico del archivo (`Ouput` en lugar de `Output`).

**Severidad: BAJO.**

## B-03 — Comentarios temporales/de depuración

Existen comentarios como:

- “faltaba este”;
- “más realista”;
- “sin datos -> solo regla”;
- comentarios con emojis de depuración.

No afectan por sí solos la ejecución, pero deben limpiarse/documentarse para portafolio.

**Severidad: BAJO.**

---

# 12. Seguridad — evaluación específica

## 12.1 Secretos

### Estado

**NO APROBADO.**

Hallazgo principal: credenciales Azure SQL en historial Git.

El proyecto ya posee `UserSecretsId`, lo cual es una buena base para mover secretos locales fuera del repositorio.

## 12.2 SQL Injection

No se encontró uso directo de SQL concatenado ni `FromSqlRaw` con entradas de usuario en el código inspeccionado.

EF Core LINQ reduce actualmente la superficie típica de SQL injection.

### Estado

**Riesgo directo observado: BAJO**, pero la validación de datos sigue siendo obligatoria.

## 12.3 XSS

Riesgo principalmente por uso de `innerHTML`.

### Estado

**MEDIO.**

## 12.4 CSRF

No se observó una estrategia antiforgery en los endpoints JSON propios.

### Estado

**ALTO.**

## 12.5 Autenticación

Identity existe, pero no gobierna el flujo de investigación.

### Estado

**INCOMPLETO.**

## 12.6 Autorización/roles

No se encontró configuración efectiva de roles ni endpoints administrativos protegidos.

### Estado

**NO IMPLEMENTADO.**

## 12.7 Privacidad de estudiantes

El sistema almacena información que puede considerarse sensible para una investigación:

- indicadores emocionales;
- resultados cognitivos;
- hábitos de uso de IA;
- logs de interacción;
- resultados de predicción.

El pseudónimo `UserToken` enlaza estos datos entre tablas.

Se requiere una política explícita de:

- minimización;
- retención;
- acceso;
- cifrado;
- backups;
- trazabilidad;
- exportación/eliminación según protocolo;
- separación de datos identificables y resultados, si existieran datos identificables en futuras fases.

### Estado

**ALTO.**

---

# 13. Estado de formularios

## Cognitivo

**Implementado como MVP.**

Problemas:

- tres intentos solamente;
- no se detectó mecanismo de guardado de cada intento;
- solo se persiste promedio;
- no se espera confirmación del backend;
- no se registra estado de evaluación;
- sin manejo robusto de abandono;
- dependencia visual del color.

## DASS-21

**Implementado como MVP.**

Problemas:

- respuestas se procesan en cliente;
- backend recibe puntajes agregados;
- no hay validación server-side del rango;
- no hay progreso;
- no hay persistencia parcial;
- no se conserva evidencia de respuestas individuales;
- debe verificarse la correspondencia exacta entre scoring implementado y protocolo metodológico.

## Uso de IA

**Implementado como MVP.**

Problemas:

- HTML limita valores pero backend no;
- `TipoUso` acepta cualquier string vía request directo;
- no hay DTO validado;
- no hay estado de guardado real.

## SUS

**Implementado como MVP.**

Problemas:

- scoring solo cliente;
- backend recibe solo puntaje total;
- no hay respuestas auditables;
- falta manejo robusto de errores.

---

# 14. Estado del flujo de evaluación

## Flujo pretendido

```text
Consentimiento
-> Cognitivo
-> Emocional
-> Uso IA
-> SUS
-> Resultado
```

## Riesgos encontrados

- JavaScript posiblemente no cargado por markup incorrecto;
- CSS con rutas incorrectas;
- guardados no siempre esperados;
- fetch no verifica error HTTP;
- no existe “evaluación en curso” como entidad;
- no existe progreso persistido;
- no existe recuperación si el usuario recarga;
- no existe prevención de salto manual entre rutas;
- controles de usuario válidos se realizan solo en algunas acciones;
- doble envío posible;
- resultado repetido genera nuevas filas;
- falta trazabilidad del ciclo completo.

### Evaluación

**Estado: INCOMPLETO / INESTABLE.**

---

# 15. Estado de Machine Learning

## Algoritmo runtime observado

`MLService` construye un pipeline con:

1. concatenación de features;
2. normalización MinMax;
3. `SdcaLogisticRegression`;
4. `TrainTestSplit` 80/20;
5. evaluación binaria cuando el test contiene ambas clases.

## Features

- Latencia
- Estrés
- Ansiedad
- Depresión
- Horas de uso
- Días de uso
- Tipo de uso “Reemplazo” binarizado

## Label

`Riesgo` es generado mediante `CalcularRiesgo`.

## Problemas principales

1. etiqueta derivada de las mismas features;
2. entrenamiento runtime durante inferencia;
3. split no reproducible;
4. posible duplicación por joins;
5. dataset mínimo de 10 es insuficiente para interpretar métricas con solidez;
6. archivo `.mlnet` no integrado con el flujo runtime;
7. no existe versión del modelo;
8. no se persiste artefacto entrenado;
9. no se documenta dataset de entrenamiento;
10. no existe pipeline separado de entrenamiento/inferencia;
11. la regla y el ML se fusionan con OR;
12. no existe trazabilidad de qué modelo produjo cada predicción.

### Evaluación

**Estado: PROTOTIPO. No listo todavía para presentarse como modelo predictivo validado.**

---

# 16. Forma actual de presentar resultados

La UI muestra principalmente:

- `ALTO`;
- `BAJO`;
- mensajes textuales.

No existe actualmente:

- nivel moderado;
- desglose cognitivo;
- desglose emocional profesional;
- contexto de la evaluación;
- versión del modelo;
- explicación de limitaciones;
- historial;
- tendencia;
- nota clara de no diagnóstico en la pantalla final.

### Problema de lenguaje

La palabra **“diagnóstico”** aparece explícitamente y debe eliminarse en una fase posterior.

### Evaluación

**Estado: NO ALINEADO completamente con el principio académico definido.**

---

# 17. Dashboard actual

## Estudiante

No se encontró dashboard de estudiante.

## Administrador

No se encontró dashboard administrativo.

## Datos existentes que podrían alimentar dashboards futuros

- consentimientos;
- resultados cognitivos;
- resultados emocionales;
- uso IA;
- logs;
- SUS;
- métricas ML;
- predicciones;
- usuarios Identity;
- usuarios de investigación.

Antes de crear dashboards debe definirse correctamente:

- modelo de evaluación;
- identidad;
- autorización;
- privacidad;
- agregaciones permitidas.

---

# 18. Problemas UI/UX

## ALTO

- recursos JS mal incluidos;
- rutas CSS incorrectas;
- falta de feedback real de errores de red;
- navegación continúa aunque falle un guardado.

## MEDIO

- no existe progress bar;
- no se muestra “paso X de Y”;
- no se indica guardado exitoso confirmado por servidor;
- falta estado loading;
- falta deshabilitar botones durante envío;
- falta recuperación tras recarga;
- falta historial;
- resultado final demasiado binario;
- mensajes potencialmente demasiado concluyentes;
- no hay dashboard.

---

# 19. Estado de Git y mantenibilidad

El ZIP contiene `.git`.

Se detectó:

- un commit principal visible;
- credencial Azure SQL en el contenido versionado del commit;
- múltiples cambios locales sin commit;
- aproximadamente 84 entradas modificadas/eliminadas/no rastreadas en `git status`;
- cambios reales importantes en vistas y estilos además de diferencias de fin de línea.

### Impacto

No existe en este momento un baseline limpio y seguro para continuar desarrollando.

### Recomendación

Antes de la siguiente publicación debe crearse un baseline seguro y revisado, evitando arrastrar el historial contaminado.

---

# 20. Dependencias y configuración

Paquetes principales observados:

- ASP.NET Core Identity 10.0.6;
- Entity Framework Core SQL Server 10.0.6;
- EF Core SQLite 10.0.6;
- Microsoft.ML 5.0.0;
- Microsoft.ML.FastTree 5.0.0;
- paquetes de scaffolding;
- paquetes NuGet auxiliares.

### Pendiente obligatorio

Ejecutar posteriormente en entorno con SDK:

```text
restore
build
test
package vulnerability audit
```

No se debe asumir que la ausencia de errores visibles en código implica que las dependencias estén libres de advisories.

---

# 21. Mejoras propuestas por prioridad

## Prioridad 0 — Incidente de secretos

- rotar/revocar definitivamente credenciales expuestas;
- historial Git limpio;
- política `.gitignore` segura;
- User Secrets en desarrollo;
- Azure Key Vault/App Settings para producción;
- secret scanning pre-commit/pre-push/CI.

## Prioridad 1 — Recuperar funcionamiento base

- corregir carga JS;
- corregir rutas CSS;
- verificar sesión;
- verificar compilación;
- ejecutar migraciones en ambiente local seguro;
- verificar flujo completo manualmente.

## Prioridad 2 — Endurecimiento del backend

- validación server-side;
- antiforgery;
- autorización;
- roles;
- manejo centralizado de errores;
- evitar exposición de excepciones;
- no retornar identificadores internos innecesarios;
- endpoints con métodos HTTP correctos.

## Prioridad 3 — Modelo de datos

- definir entidad de evaluación;
- relaciones e índices;
- integridad referencial;
- estados de evaluación;
- historial coherente.

## Prioridad 4 — ML y metodología

- verificar variable objetivo;
- justificar etiqueta;
- separar entrenamiento de inferencia;
- dataset versionado/documentado;
- seed/reproducibilidad;
- estrategia de validación;
- guardar versión de modelo;
- asociar predicción con modelo/evaluación;
- métricas únicamente cuando sean metodológicamente válidas.

## Prioridad 5 — UX de evaluación

- wizard/progreso;
- autosave o guardado seguro;
- feedback de carga/error;
- reintentos controlados;
- prevención de doble submit;
- recuperación de sesión/evaluación.

## Prioridad 6 — Resultados

- eliminar lenguaje diagnóstico;
- mostrar “resultado de evaluación”, “nivel observado” o “nivel estimado” según corresponda;
- separar indicadores cognitivos y emocionales reales;
- explicar limitaciones;
- disclaimer no diagnóstico.

## Prioridad 7 — Dashboards

Primero estudiante; luego administrador con autorización real.

## Prioridad 8 — QA y deployment

- MSTest;
- tests de integración;
- pruebas de seguridad;
- pruebas responsive;
- accesibilidad;
- performance;
- pipeline CI/CD;
- deployment seguro.

---

# 22. Roadmap recomendado

Se conserva el orden general solicitado, con una condición: los bloqueadores críticos deben resolverse antes de avanzar a diseño visual.

### Fase 1 — Auditoría inicial

**Estado: completada por inspección estática.**

### Fase 2 — Bugs críticos

Objetivo:

- recuperar carga correcta de JS/CSS;
- estabilizar sesión;
- estabilizar flujo de guardado;
- corregir duplicados evidentes.

### Fase 3 — Seguridad

Objetivo:

- repositorio seguro;
- secretos fuera de Git;
- auth/autorización;
- validación;
- antiforgery;
- headers;
- errores seguros;
- privacidad.

### Fase 4 — Refactor necesario

Objetivo:

- adelgazar `InvestigacionController`;
- separar servicios;
- eliminar código muerto;
- modelar evaluación.

### Fase 5 — Logo y sistema visual

Solo después de estabilizar la base.

### Fase 6 — Formularios

Wizard/progreso/validaciones/feedback.

### Fase 7 — Flujo de evaluación

Persistencia y trazabilidad de una evaluación completa.

### Fase 8 — Resultados

Presentación responsable y no diagnóstica.

### Fase 9 — Dashboard estudiante

Historial y estado de evaluaciones.

### Fase 10 — Dashboard administrador

Con roles, privacidad y agregaciones seguras.

### Fase 11 — Responsive

Validación en todas las resoluciones objetivo.

### Fase 12 — Pruebas

MSTest + integración.

### Fase 13 — Rendimiento

Consultas, entrenamiento, payloads y recursos.

### Fase 14 — Documentación

README y documentos por fase.

### Fase 15 — Deployment

Azure seguro y reproducible.

### Fase 16 — Portafolio

Screenshots, case study y presentación final.

---

# 23. Definición de “terminado” para la Fase 1

Esta fase puede considerarse terminada cuando:

- [x] se identificó la arquitectura actual;
- [x] se inventariaron módulos y funcionalidades;
- [x] se identificaron funcionalidades incompletas;
- [x] se identificaron bugs visibles;
- [x] se identificaron riesgos de seguridad;
- [x] se inspeccionó Git buscando exposición de secretos;
- [x] se inspeccionó el flujo de evaluación;
- [x] se inspeccionó la lógica ML;
- [x] se evaluó la forma de presentar resultados;
- [x] se revisó estado de dashboards;
- [x] se revisó UI/UX y responsive de forma estática;
- [x] se clasificaron prioridades;
- [x] se definió roadmap;
- [ ] compilación verificada en entorno con .NET SDK;
- [ ] ejecución end-to-end verificada en navegador;
- [ ] dependencias verificadas contra advisories actuales.

Los tres puntos pendientes requieren un entorno de ejecución con .NET y se realizarán antes de declarar estable la versión técnica.

---

# 24. Bloqueadores antes de crear un nuevo repositorio GitHub

**NO subir todavía el proyecto actual tal como está.**

Antes de crear el nuevo repo deben cumplirse todos:

- [ ] credencial antigua revocada/rotada en Azure;
- [ ] ningún secreto válido dentro de archivos de configuración;
- [ ] ningún secreto dentro del historial Git que se vaya a publicar;
- [ ] `.gitignore` endurecido;
- [ ] configuración local movida a User Secrets;
- [ ] configuración de producción definida mediante Azure seguro;
- [ ] publish profiles revisados;
- [ ] `git diff --cached` revisado antes del primer push;
- [ ] secret scan ejecutado;
- [ ] build limpio;
- [ ] tests básicos ejecutados;
- [ ] documentación sin secretos ni datos personales.

---

# 25. Conclusión

El proyecto **sí tiene una base útil de MVP**, pero actualmente mezcla lógica de prototipo, lógica de investigación, autenticación scaffolded y entrenamiento ML runtime de una manera que necesita estabilización antes de ampliar funcionalidades.

La prioridad inmediata no debe ser agregar pantallas nuevas. Debe ser construir una base confiable:

```text
SEGURIDAD
    -> FUNCIONAMIENTO
        -> INTEGRIDAD DE DATOS
            -> VALIDEZ DEL FLUJO ML
                -> UX
                    -> DASHBOARDS
                        -> QA
                            -> DEPLOYMENT
```

La exposición anterior de `ConexionSQLAzure` demuestra por qué la nueva versión debe adoptar **seguridad por diseño** desde el primer commit.

Hasta completar la fase de seguridad, este proyecto debe tratarse como **no apto para publicación pública ni deployment productivo**.
