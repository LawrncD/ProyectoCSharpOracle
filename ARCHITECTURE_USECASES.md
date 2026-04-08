## Arquitectura DDD - Un UseCase por Método de Domain Service

### 📋 Estructura de Casos de Uso

Cada **Caso de Uso** corresponde a un **método específico** de un **Domain Service**:

```
Domain Services (Validación)     →     Use Cases (Orquestación)     →     DAOs (Persistencia)
                                                                            
RegistroUsuariosService                 RegistroUsuarios/
├─ registrarUsuario()        ──────→   ├─ RegistrarUsuarioUseCase
├─ registrarSalida()         ──────→   └─ RegistrarSalidaUseCase

EquipoService                           Equipo/
├─ obtenerTodas()            ──────→   ├─ ObtenerTodosEquiposUseCase
├─ insertarEquipo()          ──────→   ├─ InsertarEquipoUseCase
├─ obtenerValorEquipos...()  ──────→   ├─ ObtenerValorEquiposPorConfederacionUseCase
└─ obtenerEquiposMasCaros()  ──────→   └─ ObtenerEquiposMasCarosPorCiudadUseCase

ConfederacionService                    Confederacion/
├─ obtenerTodas()            ──────→   ├─ ObtenerTodasConfederacionesUseCase
└─ obtenerValorEquipos()     ──────→   └─ ObtenerValorEquipoConfederacionUseCase
```

### 🔄 Flujo de cada Use Case

```java
UseCase.Execute(usuario, parámetros)
    ↓
1️⃣ Llamar DomainService.metodo(usuario, ...)
   ├─ AuthorizationService.ValidatePermission() ← Validación
   └─ Lógica de negocio / exceptions
    ↓
2️⃣ Si valida correctamente, llamar DAO
   └─ _dao.Operación(params) ← Persistencia/Consulta
    ↓
3️⃣ Retornar resultado
```

### 📁 Estructura de Carpetas

```
Application/
└─ UseCases/
   ├─ RegistroUsuarios/
   │  ├─ RegistrarUsuarioUseCase.cs
   │  └─ RegistrarSalidaUseCase.cs
   ├─ Equipo/
   │  ├─ ObtenerTodosEquiposUseCase.cs
   │  ├─ InsertarEquipoUseCase.cs
   │  ├─ ObtenerValorEquiposPorConfederacionUseCase.cs
   │  └─ ObtenerEquiposMasCarosPorCiudadUseCase.cs
   └─ Confederacion/
      ├─ ObtenerTodasConfederacionesUseCase.cs
      └─ ObtenerValorEquipoConfederacionUseCase.cs
```

### 💡 Ejemplo de Use Case

```csharp
public class RegistrarUsuarioUseCase
{
    private readonly RegistroUsuariosService _registroService;
    private readonly UsuarioDAO _usuarioDAO;

    // Inyección de dependencias
    public RegistrarUsuarioUseCase(RegistroUsuariosService registroService, UsuarioDAO usuarioDAO)
    {
        _registroService = registroService;
        _usuarioDAO = usuarioDAO;
    }

    // Orquestación: Domain Service → DAO
    public bool Execute(Usuario usuarioRegistrador, Usuario usuarioNuevo)
    {
        // 1. Validación de dominio (AuthorizationService.ValidatePermission() aquí)
        _registroService.registrarUsuario(usuarioRegistrador, usuarioNuevo);

        // 2. Persistencia
        return _usuarioDAO.Insertar(usuarioNuevo);
    }
}
```

### 🎯 Beneficios

- ✅ **1 a 1**: Cada método de dominio tiene su UseCase
- ✅ **Reutilizable**: El mismo UseCase desde UI, API, CLI
- ✅ **Testeable**: Mock DAOs y servicios fácilmente
- ✅ **DDD Puro**: Lógica en dominio, orquestación en aplicación
- ✅ **Mantenible**: Cambios centralizados en un lugar
- ✅ **Escalable**: Agregar nueva funcionalidad = nuevos Domain Service methods + UseCase

### 📝 Lista de Casos de Uso

| Domain Service | Método | UseCase | Operación |
|---|---|---|---|
| RegistroUsuariosService | registrarUsuario() | RegistrarUsuarioUseCase | Agregar usuario (Admin) |
| RegistroUsuariosService | registrarSalida() | RegistrarSalidaUseCase | Cerrar sesión |
| EquipoService | obtenerTodas() | ObtenerTodosEquiposUseCase | Listar equipos |
| EquipoService | insertarEquipo() | InsertarEquipoUseCase | Agregar equipo |
| EquipoService | obtenerValorEquipos...() | ObtenerValorEquiposPorConfederacionUseCase | Consultar valor por conf. |
| EquipoService | obtenerEquiposMasCaros...() | ObtenerEquiposMasCarosPorCiudadUseCase | Equipo más caro por país |
| ConfederacionService | obtenerTodas() | ObtenerTodasConfederacionesUseCase | Listar confederaciones |
| ConfederacionService | obtenerValorEquipos() | ObtenerValorEquipoConfederacionUseCase | Valor de equipos por conf. |

### 🔐 Autorización en Domain Services

Cada método del Domain Service **valida permisos** automáticamente:

```csharp
public class RegistroUsuariosService
{
    public Usuario registrarUsuario(Usuario registrador, Usuario nuevo)
    {
        AuthorizationService.ValidatePermission(registrador, Operation.AddUser);
        return nuevo;  // Si no valida, lanza excepción
    }
}
```

El MaxUseCase **NO** duplica estas validaciones, solo **orquesta** el flujo.

### 🚀 Cómo Usar desde UI

```csharp
// En el constructor del formulario (inyección de dependencias)
private readonly RegistrarUsuarioUseCase _registrarUseCase;

public FrmUsuarios(RegistrarUsuarioUseCase registrarUseCase)
{
    _registrarUseCase = registrarUseCase;
}

// En el evento del botón
private void btnAgregar_Click(object sender, EventArgs e)
{
    try
    {
        var usuarioActivo = SessionManager.Instance.UsuarioActivo;
        var nuevoUsuario = new Usuario 
        { 
            NombreUsuario = txtUsername.Text,
            ContrasenaHash = txtPassword.Text,
            Tipo = (TipoUsuario)cmbTipo.SelectedItem
        };

        // Ejecutar el Use Case
        bool resultado = _registrarUseCase.Execute(usuarioActivo, nuevoUsuario);
        
        if (resultado)
            MessageBox.Show("Usuario registrado exitosamente.");
    }
    catch (InsufficientPermissionsException ex)
    {
        MessageBox.Show($"Permisos insuficientes: {ex.Message}");
    }
}
```

### 📊 Flujo Completo de Autorización

```
Usuario (UI) → RegistrarUsuarioUseCase → RegistroUsuariosService
                                         ↓
                        AuthorizationService.ValidatePermission()
                        ├─ ¿Es Admin? SÍ ✓
                        ├─ ¿Es Admin? NO ✗
                        │  └→ InsufficientPermissionsException
                        ↓
                    Validación pasada
                        ↓
                    UsuarioDAO.Insertar()
                        ↓
                    BD Oracle
```
