# Trabajo IPO — Gestión de complejo turístico rural

Prototipo de aplicación interactiva de escritorio desarrollado con **C#**, **WPF** y **.NET Framework 4.7.2** para la asignatura **Interacción Persona-Ordenador I** (Curso 2026/2027, ESI — Universidad de Castilla-La Mancha).

**Autores:** Javier Herreros Pinilla (50 %) y Miguel Gómez-Carreño Rodríguez-Escalona (50 %).

---

## Requisitos del entorno

- Windows 10 / 11 con **.NET Framework 4.7.2** o superior.
- Visual Studio 2019 o 2022 con la carga de trabajo **Desarrollo de escritorio .NET**.
- Sin base de datos externa: todos los datos de demostración e imágenes se mantienen en memoria / recursos embebidos.

## Cómo compilar y ejecutar

1. Abrir `Trabajo_IPO.sln` en Visual Studio (o compilar por CLI con `MSBuild.exe Trabajo_IPO.sln /t:Rebuild /p:Configuration=Debug`).
2. Seleccionar la configuración `Debug` o `Release`.
3. Establecer `Trabajo_IPO` como proyecto de inicio y pulsar `F5` o `Ctrl+F5`.
4. Iniciar sesión con una de las credenciales de prueba:

| Usuario | Contraseña | Perfil |
|---|---|---|
| `admin` | `camping2026` | Laura García Martín (Gestora del complejo) |
| `javier` | `ipo2026` | Javier Herreros Pinilla (Gestor del complejo) |
| `recepcion` | `bienvenido` | María Sánchez Pérez (Recepción y gestión) |

---

## Funcionalidades implementadas

- **Autenticación y sesión (`LoginWindow`)**:
  - Validación de credenciales con mensaje de error **inline** junto al formulario y resaltado visual de campos.
  - Opción **Mostrar contraseña**, botón **Limpiar**, acceso directo a **? Ayuda (`F1`)** y botón **Salir** con confirmación.
  - Visualización en cabecera de la foto de perfil del gestor, su rol y la fecha/hora del último acceso.
  - Opción de **Cerrar sesión** (vuelve a `LoginWindow`) y **Salir** en cualquier momento previa confirmación conservadora (`No` predeterminado).
- **Gestión de Alojamientos (Parcelas y Bungalows)**:
  - Alta, edición, consulta **Master/Detail** y baja con aviso explícito si existen reservas activas o futuras.
  - **Parcelas**: categoría (Pequeña, Mediana, Deluxe, Autocaravana), tamaño (`m²`), ubicación, capacidad, servicios (agua, luz, desagüe, setos, sombra) y **tarifas por temporada** (baja, media, alta).
  - **Bungalows**: tamaño (`m²`), capacidad máxima, tarifa por noche, **estancia mínima opcional**, equipamiento (menaje, microondas, ropa de cama y toallas, Wi-Fi, terraza, aire acondicionado), descripción y **galería de fotos** (añadir/quitar imágenes).
  - Filtros combinables por tipo, estado, capacidad mínima y búsqueda textual, más cambio manual de estado.
- **Cuadrante de Disponibilidad**:
  - Vista de calendario por quincenas (`◀ Anterior` / `Siguiente ▶`) con una fila por alojamiento y filtros por tipo y estado.
  - Codificación accesible redundante (**color + icono + texto**) para los 5 estados (`Libre`, `Ocupada`, `Reservada`, `En reparación`, `Pendiente de limpieza`).
  - Creación directa de reserva haciendo clic sobre cualquier celda `✓ Libre`.
- **Gestión de Reservas**:
  - Alta, edición, cambio rápido de estado (`En curso`, `Finalizada`) y cancelación previa confirmación.
  - Validación de teléfono(s) obligatorio(s), correo electrónico opcional, fecha de salida posterior a entrada, horas estimadas, capacidad máxima, estancia mínima en bungalows, bloqueo de alojamientos `En reparación` y detección de solapamientos.
  - Cálculo dinámico en vivo de número de noches, temporada e importe estimado, más solicitudes especiales (`Cuna`, `Menaje concreto`, `Movilidad reducida` y observaciones).
- **Gestión de Empleados Temporales (Guías y Monitores)**:
  - Alta, edición y baja con aviso explícito si el empleado tiene actividades de verano asignadas.
  - Foto reciente (con selector desde archivo), nombre, apellidos, función, teléfonos, correos, idiomas, formación y restricciones por días de la semana (`Lun..Dom`).
  - Vista **Master/Detail horizontal** con ficha lateral y listado automático de actividades asignadas.
- **Gestión de Actividades de Verano**:
  - Alta, edición, consulta **Master/Detail**, filtros (área, destinatarios, día y búsqueda) y eliminación confirmada.
  - Horario, días, cupo mínimo/máximo validado, precio por hora y/o mes, materiales y asignación múltiple de monitores con **aviso automático en vivo** ante solapes horarios o días no disponibles.
- **Accesibilidad, ayuda y atajos**:
  - Ayuda contextual mediante `ToolTip` en todos los controles y ventana `AyudaWindow` (`F1`) con índice de 8 apartados y datos de autoría.
  - Adaptación de tamaño de fuente (`A-`, `A`, `A+`) y modo de **Alto Contraste** (`Contraste`).
  - Barra de estado con retroalimentación horaria tras cada operación.
  - Atajos de teclado: `F1` (Ayuda), `Ctrl+1..5` (Pestañas), `Ctrl+N` (Nuevo), `Ctrl+E` (Editar), `Supr` (Eliminar).

---

## Estructura del proyecto

- `App.xaml` / `App.xaml.cs`: recursos compartidos, paleta controlada, estilos y ventana de inicio.
- `LoginWindow.xaml` / `.cs`: autenticación de gestores.
- `MainWindow.xaml` / `.cs`: ventana principal con navegación por 5 pestañas, filtros, cuadrante de disponibilidad y fichas Master/Detail.
- `AlojamientoFormWindow.xaml` / `.cs`: formulario dinámico para parcelas y bungalows.
- `ReservaFormWindow.xaml` / `.cs`: formulario de reservas con validaciones y cálculo de importe.
- `EmpleadoFormWindow.xaml` / `.cs`: formulario de guías y monitores temporales.
- `ActividadFormWindow.xaml` / `.cs`: formulario de actividades de verano con detección de conflictos.
- `AyudaWindow.xaml` / `.cs`: ayuda por módulos, atajos de teclado e información de autoría.
- `Modelos.cs`, `DatosPrueba.cs`, `Converters.cs`: clases de dominio (`INotifyPropertyChanged`), datos de prueba y convertidores accesibles.
- `Assets/`: logotipo, avatares y fotografías embebidas como recursos WPF.
- `Bocetos/`: proyecto Balsamiq mejorado (`New Project copy.bmpr`) y vistas exportadas en PNG.
- `Memoria_Final.md` / `Memoria_Final.pdf`: memoria completa de la práctica.
- `Manual_Usuario_Video_Guion.txt`: guion detallado para la grabación del vídeo demostración.
- `Lista_Pendientes.md`: trazabilidad completa de requisitos frente al enunciado y el Hito 1.
- `leeme.txt`: guía de ejecución y credenciales exigida por el enunciado.
