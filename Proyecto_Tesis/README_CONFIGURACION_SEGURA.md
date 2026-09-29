# Configuración segura inicial

## 1. No crear Git todavía
Esta entrega está preparada para revisión local. No ejecutar `git init` hasta comprobar funcionamiento y secretos.

## 2. Configurar base de datos
Ubícate en la carpeta de `Proyecto_Tesis.csproj` y registra la cadena real solo en User Secrets:

```powershell
dotnet user-secrets set "ConnectionStrings:ConexionSQL" "TU_CADENA_REAL"
```

Verifica solamente las claves configuradas:

```powershell
dotnet user-secrets list
```

Evita compartir la salida si contiene valores sensibles.

## 3. Restaurar y ejecutar

```powershell
dotnet restore
dotnet build
dotnet run
```

## 4. Prueba mínima manual

- Abrir Inicio.
- Aceptar consentimiento.
- Completar Test Cognitivo.
- Completar DASS-21.
- Completar Uso de IA.
- Completar SUS.
- Generar resultado.
- Volver a generar sin cambiar respuestas y comprobar que no se cree otra predicción equivalente.

## 5. Antes de crear un repositorio nuevo
Revisar que no exista `.git`, `bin`, `obj`, perfiles de publicación con datos del entorno ni archivos que contengan credenciales.
