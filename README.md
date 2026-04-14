# ClientesApp

Solución compuesta por una aplicación de escritorio WinForms y una Web API REST para la gestión de clientes.

## Estructura de la solución

```
ClientesApp/
├── ClientesApp.Domain/      # Modelos y validaciones compartidos
├── ClientesApp.Api/         # Web API REST (ASP.NET Core .NET 9)
├── ClientesApp.Desktop/     # Aplicación de escritorio (WinForms .NET Framework 4.8)
└── ClientesApp.Tests/       # Tests unitarios (xUnit)
```

---

## Requisitos previos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9)
- [.NET Framework 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48)
- Visual Studio 2022 (Windows) o Rider

> **Nota:** La aplicación de escritorio (WinForms) solo puede ejecutarse en **Windows**. La API es compatible con Windows, Linux y macOS.

---

## Arrancar la Web API

### Desde Visual Studio
1. Clic derecho sobre `ClientesApp.Api` → **Establecer como proyecto de inicio**
2. Pulsa **F5** o el botón **▶**
3. Se abrirá el navegador en `https://localhost:XXXX/swagger`

### Desde terminal
```bash
cd ClientesApp.Api
dotnet run
```

Luego abre el navegador en la URL que aparezca en la consola + `/swagger`.

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
Los datos se guardan en `Data/clientes.json` dentro del directorio de la API. Se crea automáticamente si no existe.

---

## Arrancar la aplicación Desktop

### Desde Visual Studio
1. Clic derecho sobre `ClientesApp.Desktop` → **Establecer como proyecto de inicio**
2. Pulsa **F5** o el botón **▶**

### Funcionalidades
- **Agregar clientes** — rellena el formulario y pulsa "Agregar"
- **Eliminar clientes** — selecciona una fila de la tabla y pulsa "Eliminar"
- **Importar clientes** — pulsa "Importar CSV/JSON" y selecciona un fichero

### Formatos de importación

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

### Persistencia
Los datos se guardan automáticamente al cerrar la aplicación en `clientes_store.json`, ubicado en la misma carpeta que el ejecutable (`bin/Debug/`). Al volver a abrir la app se cargan automáticamente.

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
| API | ASP.NET Core 9, Minimal API + Controllers |
| Desktop | WinForms .NET Framework 4.8 |
| Dominio | .NET Standard 2.0 |
| Serialización API | System.Text.Json |
| Serialización Desktop | Newtonsoft.Json |
| Documentación API | Swashbuckle (Swagger) |
| Tests | xUnit + Moq |
