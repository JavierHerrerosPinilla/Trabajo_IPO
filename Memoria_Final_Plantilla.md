# Memoria final — Gestión de complejo turístico

## Portada

**Asignatura:** Interacción Persona-Ordenador I  
**Titulación:** Grado en Ingeniería Informática  
**Curso académico:** 2026/2027  
**Título:** Prototipo WPF para la gestión de un complejo turístico  
**Integrantes:** [Nombre y apellidos de la persona 1] y [Nombre y apellidos de la persona 2]  
**Fecha:** [dd/mm/aaaa]

> Insertar aquí el logotipo de la universidad y, si se desea, una captura
> representativa de la aplicación.

## Introducción

El objetivo de esta práctica es analizar, diseñar e implementar un prototipo
de aplicación de escritorio para la gestión de un complejo turístico rural o
camping. La aplicación está orientada a la persona encargada de gestionar los
alojamientos, las reservas, los empleados temporales y las actividades de
verano.

El prototipo prioriza la usabilidad y la reducción de la navegación. Para ello,
las funciones principales se concentran en una ventana de gestión con pestañas
para alojamientos, reservas, empleados y actividades. El sistema incluye
autenticación, datos de prueba, formularios dinámicos, validaciones, ayuda
contextual y confirmaciones antes de acciones destructivas.

El enunciado no exige persistencia real, por lo que los datos se cargan desde
colecciones en memoria. Esta decisión permite demostrar el comportamiento de
la interfaz y del tratamiento dinámico de la información sin introducir una
base de datos ajena al alcance del prototipo.

## Análisis comparativo

El estudio comparativo se realizó sobre aplicaciones y páginas web relacionadas
con campings, reservas y gestión de alojamientos turísticos:

- **Pitchup:** referencia para buscadores, filtros, fechas y presentación de
  resultados de alojamientos.
- **Camping.info:** referencia para la organización de catálogos, servicios,
  etiquetas, imágenes y características de los campings.
- **CampManager:** referencia para la gestión profesional de reservas,
  disponibilidad y operaciones de un camping.
- **Ruralgest/MisterPlan:** referencia para calendarios, alojamientos,
  clientes y organización de reservas turísticas.

Las plataformas orientadas al público destacan por la búsqueda, los filtros,
las fotografías y las fichas resumidas. Las soluciones profesionales aportan
referencias para calendarios, disponibilidad y paneles de gestión. El prototipo
combina ambos enfoques: utiliza tablas y formularios para las tareas
administrativas, pero incorpora etiquetas, iconos, tooltips y una presentación
visual clara de la información.

Como criterios de comparación se consideraron la claridad de navegación, la
visibilidad del estado de los alojamientos, la calidad de los formularios, la
prevención de errores, el feedback, la búsqueda, la organización de los datos,
el uso de imágenes e iconos y la consistencia visual.

## Análisis de requisitos

### Actores

El único actor directo es el gestor del complejo. Puede iniciar sesión,
consultar y modificar la información, gestionar reservas, administrar
empleados y actividades, consultar la ayuda y cerrar la aplicación.

### Requisitos principales

- Gestionar parcelas y bungalows.
- Consultar capacidad, precio, ubicación, servicios y estado.
- Gestionar reservas con alojamiento, fechas, cliente y ocupantes.
- Impedir fechas de salida anteriores o iguales a la entrada.
- Impedir reservas solapadas y ocupaciones superiores a la capacidad.
- Gestionar empleados temporales, guías y monitores.
- Gestionar actividades de verano, horarios, cupos y monitores.
- Realizar altas, bajas, modificaciones y consultas.
- Mostrar ayuda sensible al contexto mediante tooltips.
- Confirmar las acciones que puedan provocar pérdida de datos.
- Mostrar la información del gestor y el último acceso.
- Permitir adaptar el tamaño de fuente de la interfaz.

### Reglas de negocio

1. Solo los usuarios autenticados acceden a la ventana principal.
2. Cada alojamiento tiene un código único.
3. La fecha de salida debe ser posterior a la fecha de entrada.
4. El número de ocupantes no supera la capacidad del alojamiento.
5. No se puede reservar un alojamiento fuera de servicio.
6. No se permiten reservas activas solapadas para el mismo alojamiento.
7. Las acciones destructivas requieren confirmación.
8. Los cambios se mantienen en memoria durante la sesión.

## Bocetos de la aplicación

En esta sección se deben insertar los bocetos de baja fidelidad realizados
con Balsamiq Mockups u otra herramienta de prototipado.

### Boceto de la ventana de login

<!-- Insertar imagen aquí:
![Boceto de Login](./bocetos/login.png)
-->

### Boceto de la ventana principal

<!-- Insertar imagen aquí:
![Boceto de ventana principal](./bocetos/main-window.png)
-->

### Boceto del formulario de alojamiento

<!-- Insertar imagen aquí:
![Boceto de formulario de alojamiento](./bocetos/alojamiento-form.png)
-->

### Boceto del formulario de reserva

<!-- Insertar imagen aquí:
![Boceto de formulario de reserva](./bocetos/reserva-form.png)
-->

## Tecnología y recursos utilizados

El prototipo se ha desarrollado como una aplicación de escritorio para Windows
utilizando **C#** y **Windows Presentation Foundation (WPF)** sobre **.NET
Framework 4.7.2**. La interfaz se define mediante archivos **XAML** y la
lógica de interacción se implementa mediante code-behind en archivos **C#**.

Se han utilizado los siguientes componentes principales:

- `Window` para las ventanas de login, formularios y ayuda.
- `Grid`, `DockPanel`, `StackPanel` y `TabControl` para los layouts.
- `DataGrid` para listados tabulares.
- `ObservableCollection<T>` para actualizar dinámicamente los datos.
- `ComboBox`, `DatePicker`, `TextBox`, `CheckBox` y `PasswordBox` para los
  formularios.
- `MessageBox` para errores, confirmaciones y feedback.
- `ToolTip` para ayuda sensible al contexto.
- Recursos de texto incluidos en el propio proyecto.

No se utiliza una base de datos ni una API externa. Los datos de prueba se
definen en `DatosPrueba.cs` y se cargan en memoria al iniciar la aplicación.
El proyecto se organiza en una solución de Visual Studio con extensión
`.sln` y un proyecto WPF con extensión `.csproj`.

## Justificación del diseño de la GUI

La interfaz se ha diseñado alrededor de una ventana principal con navegación
integrada mediante un `TabControl`. Esta decisión reduce la proliferación de
ventanas y permite que las funciones principales estén disponibles desde un
mismo contexto. El encabezado mantiene visible la identidad del gestor, su rol
y la fecha del último acceso. Los layouts `Grid`, `DockPanel` y `StackPanel`
permiten distribuir los controles de manera ordenada y adaptar la ventana a
distintos tamaños.

Desde el punto de vista de las leyes de Gestalt, se aplica la ley de proximidad
agrupando los controles relacionados dentro de cada pestaña y colocando juntos
los botones de alta, modificación y eliminación. También se aplica la ley de
semejanza mediante botones homogéneos para acciones equivalentes y mediante
tablas con columnas consistentes. Los `GroupBox` de los formularios separan
visualmente la información y facilitan que el usuario identifique qué controles
pertenecen a una misma tarea.

La aplicación utiliza metáforas e iconos reconocibles: el símbolo de suma
representa la creación, el lápiz representa la modificación y la papelera
representa la eliminación. Estas metáforas se acompañan de texto para que la
comprensión no dependa únicamente del icono o del color. El azul oscuro del
encabezado proporciona una identidad visual estable, mientras que el verde del
botón Guardar destaca una acción positiva y los avisos utilizan una apariencia
diferenciada.

La prevención de errores se implementa mediante validaciones de campos,
mensajes de error y cuadros de diálogo antes de eliminar alojamientos o
reservas. En el formulario de reservas se comprueba que la salida sea posterior
a la entrada, que el número de ocupantes sea válido y que no se supere la
capacidad del alojamiento. El botón No aparece como opción predeterminada en
las confirmaciones destructivas, siguiendo un criterio conservador.

La retroalimentación se proporciona después de las operaciones mediante
mensajes como “La reserva se ha creado correctamente” o “El alojamiento se ha
actualizado correctamente”. Los `ToolTip` aportan ayuda contextual sin obligar
al usuario a abandonar la pantalla. Los campos obligatorios se identifican
mediante un asterisco y los controles se enfocan cuando el usuario debe
corregir un dato. Además, el botón de tamaño de fuente permite adaptar la
interfaz a usuarios que necesiten una presentación más grande.

## Conclusiones

El prototipo cubre las funciones principales de gestión solicitadas en el
enunciado y aplica principios de usabilidad relacionados con agrupación,
consistencia, feedback, prevención de errores y reducción de la navegación.

Como posible ampliación futura se podría incorporar persistencia mediante una
base de datos, gestión completa de empleados y actividades, fotografías reales
de perfiles y alojamientos, y un calendario visual de disponibilidad.

## Anexos

- Guion del vídeo de demostración.
- `leeme.txt`.
- Capturas de las ventanas finales.
- Código fuente del proyecto.
