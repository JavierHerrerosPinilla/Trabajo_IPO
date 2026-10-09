# Trabajo IPO — Gestión de complejo turístico

Prototipo de aplicación de escritorio desarrollado con **C#**, **WPF** y
**.NET Framework 4.7.2** para gestionar un complejo turístico o camping.

La aplicación permite trabajar con alojamientos, reservas, empleados
temporales y actividades de verano. Los datos son de demostración y se
mantienen en memoria durante la sesión, ya que el enunciado no exige
persistencia real.

## Requisitos

- Windows.
- Visual Studio 2019 o posterior.
- Carga de trabajo de Visual Studio **Desarrollo de escritorio .NET**.
- .NET Framework 4.7.2 o una versión compatible instalada.

## Cómo ejecutar el proyecto

1. Clonar el repositorio:

   ```powershell
   git clone https://github.com/JavierHerrerosPinilla/Trabajo_IPO.git
   ```

2. Abrir la solución `Trabajo_IPO.sln` en Visual Studio.
3. Seleccionar la configuración `Debug` o `Release`.
4. Establecer `Trabajo_IPO` como proyecto de inicio si Visual Studio no lo
   selecciona automáticamente.
5. Compilar la solución mediante **Compilar > Compilar solución**.
6. Ejecutar con **Depurar > Iniciar sin depurar** o pulsando `Ctrl+F5`.
7. Iniciar sesión con una de las credenciales de prueba.

La aplicación comienza en la ventana de autenticación. Después de iniciar
sesión correctamente se muestra la ventana principal de gestión.

## Credenciales de prueba

| Usuario | Contraseña |
|---|---|
| `admin` | `camping2026` |
| `javier` | `ipo2026` |
| `recepcion` | `bienvenido` |

## Funcionalidades principales

- Inicio de sesión con validación de credenciales.
- Visualización del perfil del gestor y de su último acceso.
- Alta, modificación, consulta y baja de alojamientos.
- Gestión de parcelas y bungalows.
- Alta, modificación y cancelación de reservas.
- Validación de fechas, capacidad y solapamiento de reservas.
- Consulta de empleados temporales.
- Consulta de actividades de verano.
- Ayuda contextual mediante `ToolTip`.
- Ayuda general e información de autoría.
- Confirmación antes de eliminar datos.
- Cambio entre tamaño de fuente normal y grande.

## Estructura del proyecto

### Ventanas XAML y code-behind

- **`App.xaml` / `App.xaml.cs`**  
  Configuran la aplicación WPF y establecen `LoginWindow` como ventana
  inicial.

- **`LoginWindow.xaml` / `LoginWindow.xaml.cs`**  
  Implementan la autenticación. Comprueban el usuario y la contraseña,
  muestran errores y abren la ventana principal cuando el acceso es válido.

- **`MainWindow.xaml` / `MainWindow.xaml.cs`**  
  Contienen la interfaz principal. Organizan la aplicación mediante pestañas
  para alojamientos, reservas, empleados y actividades. También gestionan la
  ayuda, el tamaño de fuente y las confirmaciones de eliminación.

- **`AlojamientoFormWindow.xaml` / `AlojamientoFormWindow.xaml.cs`**  
  Formulario reutilizable para crear y modificar parcelas y bungalows.
  Incluye validación de campos, controles específicos por tipo y prevención
  de códigos duplicados.

- **`ReservaFormWindow.xaml` / `ReservaFormWindow.xaml.cs`**  
  Formulario reutilizable para crear y modificar reservas. Valida fechas,
  ocupantes, capacidad, disponibilidad y solapamientos.

- **`AyudaWindow.xaml` / `AyudaWindow.xaml.cs`**  
  Muestran la guía rápida de uso, la ayuda contextual, los criterios de
  seguridad y la información de autoría.

### Modelos y datos

- **`Modelos.cs`**  
  Define las clases de dominio:
  `Gestor`, `Alojamiento`, `Reserva`, `EmpleadoTemporal` y `Actividad`.

- **`DatosPrueba.cs`**  
  Contiene las colecciones `ObservableCollection<T>` con los datos de
  demostración de gestores, alojamientos, reservas, empleados y actividades.

### Configuración y documentación

- **`Trabajo_IPO.sln`**  
  Solución de Visual Studio.

- **`Trabajo_IPO.csproj`**  
  Configuración del proyecto WPF, referencias, ventanas y archivos C#.

- **`App.config`**  
  Configuración de la aplicación .NET Framework.

- **`leeme.txt`**  
  Instrucciones básicas de ejecución, integrantes y credenciales de prueba.

- **`Memoria_Final_Plantilla.md`**  
  Plantilla de la memoria final con las secciones del enunciado.

- **`Properties/`**  
  Recursos, configuración y metadatos del proyecto.

## Datos y persistencia

No se utiliza una base de datos. Los registros se inicializan en
`DatosPrueba.cs` y se almacenan en colecciones en memoria. Por este motivo,
los cambios realizados durante la ejecución se pierden al cerrar la
aplicación.

## Ayuda y usabilidad

La interfaz utiliza una ventana principal con navegación mediante pestañas para
reducir la apertura de ventanas. Las acciones importantes disponen de
`ToolTip`, las operaciones destructivas solicitan confirmación y los
formularios validan los datos antes de guardarlos.

La aplicación también permite alternar entre un tamaño de fuente normal y uno
grande mediante el botón `A+` del encabezado.

## Licencia y finalidad

Proyecto académico realizado para la asignatura Interacción
Persona-Ordenador I, curso 2026/2027.
