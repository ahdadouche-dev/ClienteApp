# ClientesApp

Solución compuesta por una aplicación de escritorio WinForms y una Web API REST para la gestión de clientes. El Desktop consume la API via HTTP, por lo que ambos proyectos deben estar en ejecución simultáneamente.

## Estructura de la solución

```
ClientesApp/
├── ClientesApp.Domain/      # Modelos y validaciones compartidos
├── ClientesApp.Api/         # Web API REST (ASP.NET Core .NET 9)
├── ClientesApp.Desktop/     # Aplicación de escritorio (WinForms .NET Framework 4.8)
├── ClientesApp.Tests/       # Tests unitarios (xUnit)
└── ejemplos/
    ├── clientes_prueba.csv  # Fichero CSV de ejemplo para importación
    └── clientes_prueba.json # Fichero JSON de ejemplo para importación
```

---

## Requisitos previos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9)
- [.NET Framework 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48)
- Visual Studio 2022 (Windows) o Rider

> **Nota:** La aplicación de escritorio (WinForms) solo puede ejecutarse en **Windows**. La API es compatible con Windows, Linux y macOS.

---

## Arrancar la aplicación

> ⚠️ **Importante:** El Desktop consume la API via HTTP. Es obligatorio arrancar **primero la API** y luego el Desktop.

### Opción 1 — Arrancar ambos proyectos a la vez (recomendado)

1. Clic derecho sobre la **Solución** en el explorador de soluciones → **Propiedades**
2. Selecciona **"Proyectos de inicio múltiples"**
3. Pon `ClientesApp.Api` en **Iniciar**
4. Pon `ClientesApp.Desktop` en **Iniciar**
5. Pulsa **Aceptar**
6. Pulsa **F5**

### Opción 2 — Arrancar manualmente cada proyecto

**Paso 1 — Arrancar la API:**
```bash
cd ClientesApp.Api
dotnet run
```
O desde Visual Studio: clic derecho en `ClientesApp.Api` → **Depurar → Iniciar nueva instancia**

**Paso 2 — Arrancar el Desktop:**

Clic derecho en `ClientesApp.Desktop` → **Depurar → Iniciar nueva instancia**

---

## Web API

La API arranca en `https://localhost:7184`. Puedes acceder a la documentación interactiva en:

```
https://localhost:7184/swagger
```

### Endpoints disponibles

| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/clientes` | Lista todos los clientes |
| GET | `/clientes/{dni}` | Obtiene un cliente por DNI |
| POST | `/clientes` | Crea un nuevo cliente |
| DELETE | `/clientes/{dni}` | Elimina un cliente por DNI |

### Ejemplo POST `/clientes`
```json
{
  "dni": "12345678A",
  "nombre": "Juan",
  "apellidos": "García López",
  "fechaNacimiento": "1990-05-15",
  "telefono": "612345678",
  "email": "juan@email.com"
}
```

### Almacenamiento
Los datos se guardan en `Data/clientes.json` dentro del directorio de la API. Se crea automáticamente si no existe. Si el fichero existe pero tiene contenido inválido, reemplaza su contenido con `[]`.

---

## Aplicación Desktop

El Desktop se conecta a la API en `https://localhost:7184`. **La API debe estar arrancada** para que el Desktop funcione correctamente.

### Funcionalidades
- **Agregar clientes** — rellena el formulario y pulsa "Agregar"
- **Eliminar clientes** — selecciona una fila de la tabla y pulsa "Eliminar"
- **Importar clientes** — pulsa "Importar CSV/JSON" y selecciona un fichero de la carpeta `ejemplos/`

### Formatos de importación

Encontrarás ficheros de ejemplo en la carpeta `ejemplos/` del repositorio.

**CSV** (con cabecera):
```
dni,nombre,apellidos,fechaNacimiento,telefono,email
12345678A,Juan,García,1990-05-15,612345678,juan@email.com
```

**JSON**:
```json
[
  {
    "Dni": "12345678A",
    "Nombre": "Juan",
    "Apellidos": "García",
    "FechaNacimiento": "1990-05-15",
    "Telefono": "612345678",
    "Email": "juan@email.com"
  }
]
```

---

## Ejecutar los tests

### Desde Visual Studio
1. Ve a **Ver → Explorador de pruebas**
2. Pulsa **▶ Ejecutar todo**

### Desde terminal
```bash
cd ClientesApp.Tests
dotnet test
```

### Tests incluidos
- `ClienteValidatorTests` — validaciones de DNI, email, teléfono y fecha (5 tests)
- `ClienteServiceTests` — operaciones CRUD del servicio de la API (7 tests)

---

## Tecnologías utilizadas

| Componente | Tecnología |
|------------|------------|
| API | ASP.NET Core 9, Controllers |
| Desktop | WinForms .NET Framework 4.8 |
| Dominio | .NET Standard 2.0 |
| Comunicación Desktop↔API | HttpClient |
| Serialización API | System.Text.Json |
| Serialización Desktop | Newtonsoft.Json |
| Documentación API | Swashbuckle (Swagger) |
| Tests | xUnit + Moq |
