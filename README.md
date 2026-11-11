# 🧠 IA Cognitiva | Plataforma de Evaluación y Machine Learning

<div align="center">

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-MVC_10-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Entity Framework Core](https://img.shields.io/badge/Entity_Framework_Core-10.0.12-68217A?style=for-the-badge&logo=nuget&logoColor=white)
![ML.NET](https://img.shields.io/badge/ML.NET-5.0.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)

![SQL Server](https://img.shields.io/badge/SQL_Server-10.0.12-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3.3-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white)
![Azure](https://img.shields.io/badge/Deployment-Microsoft_Azure-0078D4?style=for-the-badge&logo=microsoftazure&logoColor=white)

</div>

> **Inteligencia artificial aplicada a la evaluación y detección temprana de indicadores cognitivos y emocionales en estudiantes universitarios.**
>
> Plataforma web de investigación desarrollada con ASP.NET Core MVC y Machine Learning, orientada a recopilar información, procesar evaluaciones y generar resultados predictivos en un entorno académico.

<p align="center">
  <a href="https://github.com/Benyarg/tesis-ia-cognitiva-ml"><strong>💻 Ver repositorio en GitHub</strong></a>
  ·
  <a href="https://plataformaia-f6gxc2bhc3g3e4cv.eastus2-01.azurewebsites.net/"><strong>🌐 Entorno demostrativo en Microsoft Azure</strong></a>
</p>

> **Aviso importante:** Los resultados generados por la plataforma tienen carácter predictivo y de detección temprana. No constituyen un diagnóstico clínico ni sustituyen una evaluación realizada por profesionales especializados.

---

## 📖 Descripción general

**IA Cognitiva** es una plataforma web de investigación que integra técnicas de Machine Learning para la predicción y detección temprana de indicadores cognitivos y emocionales en estudiantes universitarios.

El proyecto nace de la necesidad de desarrollar una herramienta tecnológica que permita recopilar y analizar información procedente de diferentes evaluaciones, integrando variables cognitivas, emocionales y relacionadas con el uso de inteligencia artificial.

La solución está desarrollada con **ASP.NET Core MVC sobre .NET 10, Entity Framework Core, ML.NET y SQL Server**, utilizando una arquitectura web que integra la gestión de participantes, el procesamiento de evaluaciones y la administración de modelos predictivos.

El resultado es una plataforma que centraliza el flujo de investigación, desde el consentimiento informado y la recopilación de respuestas hasta la generación de resultados y el análisis mediante Machine Learning.

---

## ✨ Características principales

### 1. Flujo de evaluación de participantes

La plataforma incorpora un proceso estructurado para recopilar información y ejecutar las diferentes evaluaciones contempladas en la investigación.

- **Consentimiento informado:** registro de la aceptación de las condiciones de participación.
- **Prueba cognitiva:** recopilación de información relacionada con indicadores de desempeño cognitivo y tiempos de respuesta.
- **Evaluación emocional:** registro de indicadores relacionados con estrés, ansiedad y depresión.
- **Cuestionario de uso de IA:** recopilación de información sobre frecuencia y modalidades de utilización de herramientas de inteligencia artificial.
- **Evaluación SUS:** aplicación del cuestionario _System Usability Scale_ para recoger información sobre la percepción de usabilidad.
- **Generación de resultados:** procesamiento y presentación de los resultados obtenidos durante el flujo de evaluación.

### 2. Panel administrativo

La aplicación dispone de un entorno de administración protegido para supervisar la información y gestionar los componentes de investigación.

- **Dashboard administrativo:** visualización de indicadores generales de la plataforma.
- **Gestión de participantes:** consulta y seguimiento de participantes mediante identificadores tokenizados.
- **Seguimiento de evaluaciones:** revisión del estado de los procesos cognitivos, emocionales, de uso de IA y usabilidad.
- **Resultados de predicción:** consulta de los niveles de riesgo generados por el modelo.
- **Historial de modelos:** visualización de los resultados de entrenamiento y evaluación, incluyendo métricas como Accuracy, Precision, Recall y F1-Score.
- **Control de acceso:** autenticación y autorización mediante ASP.NET Core Identity y roles administrativos.

### 3. Integración de Machine Learning

La plataforma incorpora un módulo de Machine Learning desarrollado con ML.NET para entrenar y evaluar modelos predictivos utilizando los datos disponibles en el entorno correspondiente.

- **Entrenamiento del modelo:** procesamiento de los datos disponibles para generar modelos predictivos.
- **Evaluación:** cálculo y registro de métricas de rendimiento.
- **Resultados:** almacenamiento de métricas y resultados asociados a las ejecuciones del modelo.
- **Predicciones:** generación de resultados clasificados según los niveles definidos por la aplicación.
- **Administración:** integración de las operaciones relacionadas con modelos dentro del panel administrativo.

> Las métricas obtenidas en el entorno demostrativo no representan la validación experimental definitiva de la investigación ni deben interpretarse como evidencia de rendimiento clínico.

### 4. Seguridad y control de acceso

- **Autenticación:** implementación de ASP.NET Core Identity.
- **Autorización:** protección de operaciones administrativas mediante roles.
- **Antiforgery:** validación global contra solicitudes falsificadas mediante `AutoValidateAntiforgeryToken`.
- **Rate Limiting:** limitación de solicitudes para el acceso administrativo y los procesos de evaluación.
- **Protección de sesiones:** configuración de cookies `HttpOnly`, `Secure` y `SameSite`.
- **Políticas de contraseñas:** requisitos de complejidad y longitud mínima.
- **Bloqueo de cuentas:** control de intentos fallidos de autenticación.
- **Cabeceras HTTP:** incorporación de políticas de seguridad para los recursos del navegador.

### 5. Automatización y mantenimiento

- **Pruebas automatizadas:** proyecto independiente de pruebas basado en MSTest.
- **Integración continua:** automatización de validaciones mediante GitHub Actions.
- **Dependabot:** supervisión y actualización de dependencias.
- **Gestión de configuración:** separación de secretos y credenciales mediante User Secrets y variables de entorno.

---

## 🛠️ Tecnologías utilizadas

### Backend y persistencia

| Tecnología             | Versión | Aplicación                                   |
| ---------------------- | ------: | -------------------------------------------- |
| .NET                   |    10.0 | Plataforma de ejecución y desarrollo.        |
| ASP.NET Core MVC       |    10.0 | Framework para la aplicación web.            |
| C#                     |       — | Lenguaje de programación backend.            |
| Entity Framework Core  | 10.0.12 | ORM y acceso a datos.                        |
| ASP.NET Core Identity  | 10.0.12 | Autenticación y administración de usuarios.  |
| SQL Server Provider    | 10.0.12 | Conexión y persistencia mediante SQL Server. |
| SQL Server / Azure SQL |       — | Sistema de gestión de base de datos.         |

### Inteligencia artificial y Machine Learning

| Tecnología      | Versión | Aplicación                                                 |
| --------------- | ------: | ---------------------------------------------------------- |
| ML.NET          |   5.0.0 | Entrenamiento y evaluación de modelos de Machine Learning. |
| ML.NET FastTree |   5.0.0 | Biblioteca de algoritmos FastTree para ML.NET.             |

### Frontend y presentación

| Tecnología  |         Versión | Aplicación                                      |
| ----------- | --------------: | ----------------------------------------------- |
| Razor Views | ASP.NET Core 10 | Generación de interfaces dinámicas.             |
| HTML5       |               — | Estructura semántica.                           |
| CSS3        |               — | Estilos y diseño responsive.                    |
| JavaScript  |               — | Interactividad y comportamiento de la interfaz. |
| Bootstrap   |           5.3.3 | Componentes visuales y diseño adaptable.        |
| jQuery      |           3.7.1 | Manipulación del DOM e interacciones.           |

### Pruebas y calidad de software

| Tecnología             | Versión | Aplicación                               |
| ---------------------- | ------: | ---------------------------------------- |
| MSTest Framework       |   4.4.1 | Desarrollo de pruebas automatizadas.     |
| MSTest Test Adapter    |   4.4.1 | Descubrimiento y ejecución de pruebas.   |
| Microsoft.NET.Test.Sdk | 18.10.1 | Infraestructura de ejecución de pruebas. |

### Herramientas y despliegue

| Tecnología          | Versión | Aplicación                                              |
| ------------------- | ------: | ------------------------------------------------------- |
| EF Core Tools       | 10.0.12 | Herramientas de administración de Entity Framework.     |
| Web Code Generation |  10.0.2 | Herramientas de generación de código para ASP.NET Core. |
| NuGet.Packaging     |   7.9.0 | Gestión de paquetes NuGet.                              |
| NuGet.Protocol      |   7.9.0 | Integración con protocolos de NuGet.                    |
| Git                 |       — | Control de versiones.                                   |
| GitHub              |       — | Alojamiento del repositorio.                            |
| GitHub Actions      |       — | Integración continua.                                   |
| Dependabot          |       — | Supervisión de dependencias.                            |
| Microsoft Azure     |       — | Plataforma de despliegue y demostración.                |

---

## 🏗️ Arquitectura del proyecto

La solución sigue una arquitectura basada en ASP.NET Core MVC, separando las responsabilidades de presentación, persistencia, seguridad, servicios y lógica de la investigación.

```text
tesis-ia-cognitiva-ml/
│
├── .github/
│   └── workflows/                 # Automatización con GitHub Actions
│
├── Proyecto_Tesis/
│   ├── Controllers/               # Controladores MVC
│   ├── Data/                      # ApplicationDbContext y persistencia
│   ├── Models/                    # Entidades y modelos de dominio
│   ├── Security/                  # Roles y configuración de seguridad
│   ├── Services/                  # Servicios de aplicación y ML
│   ├── ViewModels/                # Modelos para las vistas
│   ├── Views/                     # Vistas Razor
│   ├── wwwroot/
│   │   ├── css/                   # Estilos personalizados
│   │   ├── js/                    # Scripts de frontend
│   │   ├── img/                   # Recursos gráficos
│   │   └── lib/                   # Bootstrap y jQuery
│   ├── Program.cs                 # Configuración y arranque
│   ├── appsettings.json           # Configuración general
│   └── Proyecto_Tesis.csproj      # Dependencias y framework
│
├── Proyecto_Tesis.Tests/          # Pruebas automatizadas
│   └── Proyecto_Tesis.Tests.csproj
│
├── .gitignore
├── Proyecto_Tesis.slnx            # Solución de Visual Studio
├── README.md
└── LICENSE
```

### Proyecto_Tesis

Contiene la aplicación web principal y sus componentes organizados por responsabilidades.

- **Controllers:** reciben las solicitudes HTTP y coordinan las operaciones de la aplicación.
- **Data:** contiene el contexto de Entity Framework Core y la configuración de persistencia.
- **Models:** define las entidades utilizadas para almacenar participantes, consentimientos y resultados.
- **Security:** administra componentes de seguridad y creación inicial del administrador.
- **Services:** contiene los servicios de procesamiento y lógica de aplicación.
- **ViewModels:** organiza los datos que consumen las vistas.
- **Views:** contiene las interfaces desarrolladas con Razor.
- **wwwroot:** almacena estilos, scripts, bibliotecas y recursos estáticos.

### Persistencia de datos

La aplicación utiliza `ApplicationDbContext`, que hereda de `IdentityDbContext` e integra las entidades necesarias para administrar la información de investigación.

Entre los principales conjuntos de datos se encuentran:

- `UsuariosInvestigacion`
- `Consentimientos`
- `ResultadosCognitivos`
- `ResultadosEmocionales`
- `UsoIA`
- `Logs`
- `ResultadosML`
- `ResultadosSUS`
- `ResultadosPrediccion`

### Proyecto_Tesis.Tests

Contiene las pruebas automatizadas desarrolladas con MSTest, manteniendo la validación del software separada de la aplicación principal.

---

## 🧠 Decisiones de ingeniería

Durante el desarrollo se priorizaron criterios de seguridad, integridad de datos, separación de responsabilidades y trazabilidad de los procesos de investigación.

- **Arquitectura MVC:** separación de controladores, modelos y vistas para organizar las responsabilidades de la aplicación.
- **Persistencia mediante Entity Framework Core:** administración de entidades y relaciones con SQL Server.
- **Autenticación centralizada:** utilización de ASP.NET Core Identity para controlar el acceso administrativo.
- **Servicios especializados:** separación de las operaciones de administración y gestión del modelo predictivo.
- **Identificación mediante tokens:** utilización de identificadores tokenizados para relacionar los registros de evaluación de cada participante.
- **Procesamiento predictivo:** integración de ML.NET para entrenar y evaluar modelos con los datos disponibles.
- **Seguridad por capas:** incorporación de antiforgery, rate limiting, políticas de cookies y cabeceras HTTP.
- **Configuración segura:** separación de credenciales y cadenas de conexión mediante mecanismos específicos de cada entorno.
- **Pruebas automatizadas:** utilización de MSTest para validar el comportamiento de la aplicación.

> La utilización de identificadores tokenizados contribuye a separar los registros de evaluación de los datos identificativos, pero no debe confundirse automáticamente con una anonimización irreversible.

---

## 🔒 Seguridad y protección de datos

Debido a la naturaleza de la información procesada por la plataforma, se incorporan mecanismos destinados a proteger los accesos, las operaciones y la configuración del sistema.

### Autenticación y autorización

- ASP.NET Core Identity.
- Autorización mediante roles.
- Rol administrativo gestionado mediante `AdminSeeder`.
- Longitud mínima de contraseña de 12 caracteres.
- Requisitos de complejidad de contraseñas.
- Bloqueo de autenticación después de cinco intentos fallidos durante un periodo de 15 minutos.

### Protección de solicitudes

- Validación antiforgery global.
- Rate limiting para el acceso administrativo.
- Rate limiting para los procesos de evaluación.
- Configuración de cookies seguras con `HttpOnly`, `Secure` y `SameSite=Strict`.
- Control de acceso a recursos administrativos.

### Cabeceras de seguridad

La aplicación incorpora políticas HTTP que incluyen:

- `Content-Security-Policy`
- `X-Content-Type-Options`
- `X-Frame-Options`
- `Referrer-Policy`
- `Permissions-Policy`
- `Strict-Transport-Security` en producción.

### Gestión de credenciales

- User Secrets para desarrollo local.
- Variables de entorno para producción.
- Separación de cadenas de conexión y credenciales administrativas.
- Exclusión de información sensible del control de versiones.

La información relacionada con participantes debe tratarse conforme al consentimiento informado, las condiciones de la investigación y las normas aplicables de protección de datos.

---

## 🤖 Machine Learning

El sistema integra ML.NET para entrenar y evaluar modelos de aprendizaje automático a partir de la información disponible en el entorno correspondiente.

### Flujo de procesamiento

```text
Datos disponibles
       │
       ▼
Preparación y procesamiento
       │
       ▼
Entrenamiento del modelo
       │
       ▼
Evaluación del modelo
       │
       ▼
Cálculo de métricas
       │
       ▼
Registro de resultados
       │
       ▼
Generación de predicciones
       │
       ▼
Presentación de resultados
```

### Métricas de evaluación

El panel administrativo contempla el registro de las siguientes métricas:

| Métrica   | Descripción                                     |
| --------- | ----------------------------------------------- |
| Accuracy  | Proporción de predicciones correctas.           |
| Precision | Proporción de predicciones positivas correctas. |
| Recall    | Capacidad de identificar los casos positivos.   |
| F1-Score  | Media armónica entre Precision y Recall.        |

### Consideraciones sobre la validación

Es importante diferenciar entre los resultados obtenidos durante el desarrollo y las conclusiones experimentales de la investigación.

- Los datos generados en la instancia demostrativa no forman parte automáticamente de la muestra oficial.
- Las métricas del entorno demostrativo no representan la validación experimental definitiva.
- Las predicciones son indicadores de carácter orientativo y no diagnósticos clínicos.
- La interpretación de los resultados debe considerar las limitaciones del modelo y el contexto de la investigación.

---

## ⚙️ Instalación y ejecución

### Requisitos previos

- .NET SDK 10.0.
- SQL Server local o acceso a una instancia de Azure SQL.
- Git.
- Editor compatible con proyectos ASP.NET Core y .NET 10.

### 1. Clonar el repositorio

```bash
git clone https://github.com/Benyarg/tesis-ia-cognitiva-ml.git
```

### 2. Acceder al directorio

```bash
cd tesis-ia-cognitiva-ml
```

### 3. Restaurar dependencias

```bash
dotnet restore Proyecto_Tesis.slnx
```

### 4. Compilar la solución

```bash
dotnet build Proyecto_Tesis.slnx
```

---

## 🔐 Configuración de User Secrets

Las credenciales y cadenas de conexión deben mantenerse fuera del repositorio.

El proyecto utiliza User Secrets para almacenar la configuración sensible durante el desarrollo local.

### Configurar la conexión a SQL Server

La aplicación utiliza la clave `ConnectionStrings:ConexionSQL`.

Puedes configurarla desde la terminal:

```bash
dotnet user-secrets set "ConnectionStrings:ConexionSQL" "TU_CADENA_DE_CONEXION" --project Proyecto_Tesis/Proyecto_Tesis.csproj
```

La cadena de conexión debe apuntar a una instancia de SQL Server o Azure SQL configurada para el entorno correspondiente.

### Configurar el administrador inicial

La creación de la cuenta administrativa utiliza los siguientes parámetros:

```bash
dotnet user-secrets set "AdminSeed:Email" "TU_CORREO_ADMIN" --project Proyecto_Tesis/Proyecto_Tesis.csproj

dotnet user-secrets set "AdminSeed:Password" "TU_PASSWORD_SEGURO" --project Proyecto_Tesis/Proyecto_Tesis.csproj
```

No publiques valores reales de conexión, contraseñas ni credenciales administrativas.

---

## ▶️ Ejecutar la aplicación

Para iniciar el servidor de desarrollo:

```bash
dotnet run --project Proyecto_Tesis/Proyecto_Tesis.csproj
```

Abre en el navegador la dirección local indicada en la terminal.

---

## 🧪 Pruebas y calidad de software

El proyecto dispone de un proyecto independiente de pruebas automatizadas basado en MSTest.

### Ejecutar todas las pruebas de la solución

```bash
dotnet test Proyecto_Tesis.slnx
```

### Ejecutar las pruebas del proyecto específico

```bash
dotnet test Proyecto_Tesis.Tests/Proyecto_Tesis.Tests.csproj
```

Las pruebas automatizadas permiten verificar el comportamiento de los componentes implementados y reducir regresiones durante el mantenimiento del proyecto.

### Integración continua

El repositorio incorpora herramientas de automatización orientadas a mantener la calidad del código y supervisar las dependencias:

- **GitHub Actions:** integración continua.
- **Dependabot:** detección de actualizaciones de dependencias.
- **MSTest:** validación automatizada de funcionalidades.

---

## 🌐 Entorno demostrativo y despliegue

La plataforma dispone de una instancia pública en **Microsoft Azure**, destinada exclusivamente a demostración y pruebas de funcionamiento.

El entorno demostrativo permite explorar el funcionamiento general de la aplicación sin que sus datos se incorporen automáticamente a la muestra oficial de la investigación.

### Despliegue

La configuración de producción contempla:

- Aplicación ASP.NET Core MVC sobre .NET 10.
- Persistencia mediante SQL Server / Azure SQL.
- Administración de secretos mediante la configuración de Azure.
- Publicación del proyecto en Microsoft Azure.

**URL de demostración:** agrega aquí la dirección pública de tu aplicación Azure.

---

## 🏆 Resultados y alcance del proyecto

IA Cognitiva representa una solución tecnológica que integra desarrollo web, gestión de información y Machine Learning dentro de un entorno académico de investigación.

El proyecto permite:

- Centralizar el proceso de evaluación de participantes.
- Recopilar información mediante diferentes instrumentos de evaluación.
- Organizar los resultados cognitivos, emocionales y de uso de IA.
- Administrar participantes y resultados desde un panel protegido.
- Entrenar y evaluar modelos de Machine Learning.
- Generar resultados predictivos a partir de la información disponible.
- Incorporar mecanismos de seguridad y automatización durante el desarrollo.

La plataforma constituye una herramienta de apoyo a la investigación y exploración de indicadores cognitivos y emocionales, sin sustituir evaluaciones clínicas ni profesionales.

---

## 📚 Documentación

La documentación técnica complementaria y los registros de auditoría del proyecto se conservan como material de referencia para comprender su evolución y las decisiones adoptadas durante el desarrollo.

La auditoría inicial se mantiene como registro histórico de la evolución técnica del sistema.

---

## 👨‍💻 Autor

**Benjamin Rumay** · Software Developer

Diseño y desarrollo de software enfocado en la creación de soluciones tecnológicas, integración de inteligencia artificial y desarrollo de aplicaciones web orientadas a necesidades reales.

[![GitHub](https://img.shields.io/badge/GitHub-Benyarg-181717?style=for-the-badge&logo=github)](https://github.com/Benyarg)

---

## 📄 Licencia

Este proyecto utiliza la licencia **MIT**, aplicable exclusivamente al código fuente cuyos derechos de distribución corresponden al titular indicado en el archivo `LICENSE`.

Los datos de investigación, información de participantes, registros de evaluaciones, instrumentos de investigación, documentos académicos, contenidos de terceros y modelos entrenados con datos sujetos a restricciones no están automáticamente incluidos en esta concesión.

Consulta el archivo [LICENSE](./LICENSE) para conocer los términos completos.
