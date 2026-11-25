# Memoria Final — Gestión de Complejo Turístico Rural

## 1. Portada

- **Universidad:** Universidad de Castilla-La Mancha (UCLM) — Escuela Superior de Informática (Ciudad Real)
- **Titulación:** Grado en Ingeniería Informática
- **Asignatura:** Interacción Persona-Ordenador I
- **Curso académico:** 2026 / 2027
- **Título de la práctica:** Prototipo de aplicación interactiva de escritorio (WPF) para la gestión de un complejo turístico rural o camping
- **Integrantes y porcentaje de participación:**
  - **Javier Herreros Pinilla** — 50 %
  - **Miguel Gómez-Carreño Rodríguez-Escalona** — 50 %
- **Versión:** Prototipo final 2.0

---

## 2. Introducción: objeto y delimitación

El objeto de esta práctica es analizar, diseñar e implementar un **prototipo de aplicación de escritorio con interfaz gráfica de usuario (GUI) en WPF** (*Windows Presentation Foundation*, C# y .NET Framework 4.7.2) para la persona encargada de gestionar un **complejo turístico rural o camping**.

Siguiendo el enfoque de **Diseño Centrado en el Usuario (DCU)** y el modelo de proceso iterativo **MPIu+a** (*Análisis de Requisitos → Diseño → Implementación → Lanzamiento*, apoyados en **Prototipado** y **Evaluación**), el prototipo prioriza la usabilidad, la reducción de la navegación, la prevención de errores, el feedback continuo y la claridad visual sobre la complejidad de backend.

### Delimitación del alcance
- **Único rol directo:** persona gestora del complejo (pueden existir varios usuarios gestores, cada uno con credenciales, foto de perfil y registro de último acceso).
- **Áreas funcionales cubiertas:** autenticación, gestión de alojamientos (parcelas y bungalows), cuadrante de disponibilidad por fechas, gestión de reservas, gestión de empleados temporales (guías y monitores), gestión de actividades de verano, adaptación accesible (tamaño de fuente y alto contraste) y sistema de ayuda/autoría.
- **Persistencia en memoria:** tal como indica el enunciado, no se conecta a una base de datos externa; los datos de prueba e imágenes se incluyen como recursos del proyecto y se gestionan dinámicamente mediante `ObservableCollection<T>` e `INotifyPropertyChanged`.
- **Fuera de alcance:** reservas directas realizadas por clientes vía web, pasarelas de pago o facturación fiscal.

---

## 3. Análisis comparativo de aplicaciones del dominio

De acuerdo con el estudio previo realizado en el Hito 1, se analizaron tres referentes complementarios del sector bajo criterios de navegación, visibilidad de estado, formularios, filtros, feedback, uso de iconos y consistencia:

| Aplicación | Qué ofrece | Aspectos positivos adoptados | Aspectos mejorables evitados |
|---|---|---|---|
| **Pitchup** (portal de campings) | Buscador de disponibilidad con filtros por tipo de camping, tamaño, parcela e instalaciones. | Flujo claro de filtrado combinable por tipo, estado y capacidad, y fichas resumidas legibles. | Exceso de filtros simultáneos pensados para cliente final, no para recepción. |
| **Booking.com** (portal generalista) | Fichas de alojamiento con galería de fotos, capacidad, tarifa por noche y selector de calendario. | Jerarquía visual clara, precio/capacidad visibles, selector `DatePicker` y galería de fotos en bungalows. | Sobrecarga visual con avisos de urgencia y ofertas que distraen de la tarea administrativa. |
| **Campspot** (software profesional) | PMS para campings con mapa/cuadrante de parcelas por estado, calendario y panel de limpieza. | Cuadrante de disponibilidad por fechas y estados operativos (incluyendo limpieza y reparación). | Exceso de módulos complejos para una primera versión y riesgo de depender solo del color. |

**Síntesis aplicada al prototipo:** se adopta una ventana principal única con 5 pestañas, filtros combinables inspirados en Pitchup, fichas Master/Detail y galería de fotos inspiradas en Booking.com, y un cuadrante de disponibilidad con estados operativos inspirado en Campspot, pero corrigiendo su accesibilidad mediante **codificación redundante (color + icono + texto)**.

---

## 4. Análisis de requisitos y casos de uso

### 4.1 Actores
- **Gestor del complejo (actor directo):** inicia sesión, gestiona alojamientos, consulta y actualiza disponibilidad, gestiona reservas, administra empleados temporales y actividades de verano, consulta la ayuda y cierra sesión o sale de la aplicación.
- **Cliente y Guía/Monitor (actores indirectos):** no operan directamente la aplicación; sus datos se registran en reservas, fichas de personal y asignación a actividades.

### 4.2 Diagrama de casos de uso
*(Representado mediante diagrama UML de casos de uso en `Memoria_Final.pdf` cubriendo: Iniciar sesión, Gestionar alojamientos, Consultar disponibilidad, Gestionar reservas [`<<include>>` Consultar disponibilidad], Gestionar empleados temporales, Gestionar actividades de verano, Consultar ayuda y autoría, y Cerrar sesión / salir).*

### 4.3 Catálogo final de requisitos cubiertos
- **Autenticación (`AUT-01` a `AUT-06`):** login con validación inline, múltiples gestores con foto y último acceso, cierre de sesión y salida confirmada en cualquier momento.
- **Alojamientos (`ALO-01` a `ALO-15`):** alta, modificación, consulta Master/Detail, filtros combinables y baja (avisando si hay reservas futuras) de **parcelas** (categoría, tamaño m², ubicación, servicios y tarifas por temporada baja/media/alta) y **bungalows** (tamaño, capacidad, precio/noche, estancia mínima opcional, equipamiento, descripción y galería de fotos).
- **Disponibilidad (`DIS-01` a `DIS-05`):** 5 estados (`Libre`, `Ocupada`, `Reservada`, `En reparación`, `Pendiente de limpieza`), cambio manual, cuadrante quincenal interactivo, leyenda accesible con color + icono + texto y filtros.
- **Reservas (`RES-01` a `RES-18`):** alta, edición, cambio rápido de estado y cancelación confirmada; validación de teléfono obligatorio y email opcional, fechas (`Salida > Entrada`), horas estimadas, capacidad máxima, estancia mínima en bungalows, bloqueo de alojamientos en reparación, anti-solapamiento, solicitudes especiales e importe estimado por temporada.
- **Empleados temporales (`EMP-01` a `EMP-09`):** alta, edición, consulta Master/Detail horizontal con foto y actividades asignadas, filtros (función, idioma, búsqueda), restricciones por día de la semana y baja con aviso si tiene actividades vinculadas.
- **Actividades de verano (`ACT-01` a `ACT-12`):** alta, edición, consulta Master/Detail, filtros (área, destinatarios, día, búsqueda), cupo mín/máx, precio hora y/o mes, materiales, asignación de múltiples monitores y aviso automático en vivo ante solapes o indisponibilidad.
- **Transversales (`TRA-01` a `TRA-13`):** ventana principal con pestañas, atajos de teclado (`F1`, `Ctrl+1..5`, `Ctrl+N`, `Ctrl+E`, `Supr`), tooltips en todos los controles, confirmaciones conservadoras (`No` por defecto), barra de estado con feedback horario y adaptación accesible (`A-`/`A`/`A+` + Alto Contraste).

### 4.4 Reglas de negocio principales
1. Solo los gestores autenticados acceden a la ventana principal.
2. El código de cada alojamiento y de cada reserva es único.
3. No se permiten reservas sobre alojamientos `En reparación`, con ocupantes superiores a la capacidad, incumpliendo la estancia mínima del bungalow o solapadas con otra reserva activa.
4. Las fechas de salida deben ser estrictamente posteriores a las de entrada.
5. En actividades, el cupo máximo debe ser mayor o igual que el mínimo y debe existir al menos una tarifa (hora o mes).
6. Las acciones destructivas o de salida requieren confirmación explícita con **No** como botón predeterminado.

---

## 5. Bocetos de la aplicación (Balsamiq Mockups)

Los bocetos originales en `Bocetos/New Project copy.bmpr` fueron revisados y ampliados a **13 tableros completos** para alinearlos con los requisitos reales del enunciado, el estudio comparativo del punto 4 del Hito 1 y la teoría de IPO:
- Se corrigieron errores de los bocetos iniciales (como incluir tipos erróneos «Apartamento» o «Tienda de campaña» en lugar de Parcelas/Bungalows, marcar el email como obligatorio en reservas cuando el enunciado exige teléfono obligatorio y email opcional, o usar formato de fecha `mm/dd/yyyy`).
- Se incorporaron los tableros que faltaban (`Disponibilidad`, `Reservas`, `Nuevo bungalow`, `Empleados`, `Nuevo empleado`, `Actividades`, `Nueva actividad` y el storyboard navegacional en `General`).
- Se incluyó codificación redundante de estados (color + icono + texto), agrupación Gestalt por bloques (`GroupBox`), asteriscos `*` en campos obligatorios, mensajes de error inline junto al campo afectado, encabezados consistentes con perfil de gestor y barra de estado con feedback y atajos.

---

## 6. Tecnología y recursos utilizados

- **Lenguaje y framework:** C# y **Windows Presentation Foundation (WPF)** sobre **.NET Framework 4.7.2**.
- **IDE y compilación:** Visual Studio 2022 (`Trabajo_IPO.sln`, `Trabajo_IPO.csproj`, MSBuild 17).
- **Arquitectura de interfaz y datos:**
  - `App.xaml`: diccionario de recursos dinámicos (`DynamicResource`), paleta semántica controlada y estilos unificados.
  - `Modelos.cs`: clases de dominio con `INotifyPropertyChanged` (`NotifiableBase`) para refresco automático de la vista.
  - `Converters.cs`: `EstadoToBackgroundConverter`, `EstadoToForegroundConverter`, `EstadoToIconConverter`, `ImagePathConverter` y `NullOrEmptyToVisibilityConverter`.
  - `DatosPrueba.cs`: colecciones `ObservableCollection<T>` en memoria y lógica compartida de temporadas, importes, disponibilidad diaria y conflictos de monitores.
  - `Assets/`: 16 recursos PNG embebidos en el ensamblado (`logo_camping.png`, avatares de gestores y empleados, y fotografías de bungalows y parcelas).

---

## 7. Justificación del diseño de la GUI conectada con la teoría de IPO

### 7.1 Organización espacial, layouts y Leyes de Gestalt (`T2-FactorHumano-1Parte`)
- **Minimización de la navegación y layouts adaptables:** toda la operativa principal se concentra en una única `MainWindow` estructurada con `DockPanel`, `Grid` proporcional (`*` y `Auto`) y `TabControl`, permitiendo redimensionar las ventanas sin pérdida de legibilidad.
- **Leyes de Gestalt aplicadas conjuntamente:**
  - **Proximidad y Región Común:** todos los formularios (`AlojamientoFormWindow`, `ReservaFormWindow`, `EmpleadoFormWindow`, `ActividadFormWindow`) agrupan los controles semánticamente relacionados dentro de `GroupBox` con bordes diferenciados (ej. *Persona que reserva*, *Estancia y alojamiento*, *Solicitudes especiales*). Las barras de acciones (`Nuevo`, `Editar`, `Eliminar`) se sitúan juntas a la izquierda y los filtros combinables juntos a la derecha.
  - **Similitud:** las 5 pestañas comparten idéntica estructura superior (barra de herramientas + filtros), tabla central (`DataGrid`) con filas alternas y ficha inferior/lateral **Master/Detail**, reduciendo la curva de aprendizaje (*Ley de Jakob*).
  - **Figura/Fondo y espacio en blanco:** tarjetas blancas `#FFFFFF` sobre fondo neutro `#F5F7FA`, garantizando separación clara de planos y evitando saturación visual (*Estética y diseño minimalista*).

### 7.2 Memoria humana, carga cognitiva y modelos mentales (`T2-FactorHumano-2Parte`)
- **Número mágico de Miller (7 ± 2):** la navegación superior se limita a **5 pestañas** (dentro del rango óptimo 3–8), evitando menús profundos o anidados (`T6-EstilosInt`: ideal ≤ 2 niveles).
- **Reconocimiento frente a recuerdo:** se emplean listas desplegables (`ComboBox`), selectores visuales (`DatePicker`), casillas de verificación (`CheckBox`), **miniaturas de fotos (*thumbnails*)** en la galería de bungalows y el nombre/capacidad visible directamente en el selector de alojamientos de la reserva. Los filtros muestran siempre la selección activa y el contador (`X de Y registros`).
- **Alineación con el modelo mental:** la aplicación adopta la metáfora operativa de recepción de un complejo turístico (cuadrante por fechas, fichas de unidad y partes de limpieza/reparación).

### 7.3 Uso del color, tipografía y accesibilidad (`T2-FactorHumano-1Parte`, `T3-Metodos-PrinEstandGuides`)
- **Paleta controlada (4–7 colores) y semántica fija:** azul corporativo `#17324D` / `#1E6091` para estructura y acción primaria (*Efecto Von Restorff* en el botón de creación/acceso), verde `#2E7D32` para acciones positivas (`Guardar`, estado `Libre`), rojo `#C62828` reservado para errores y acciones destructivas, ámbar `#B26A00` para advertencias (`En reparación`, conflictos de monitor) y morado `#6A1B9A` para `Pendiente de limpieza`. Se evitan combinaciones oponentes dañinas (como texto rojo sobre fondo verde o azul sobre amarillo).
- **Codificación redundante (accesibilidad ante daltonismo):** siguiendo las guías de color de `T2-1` y las conclusiones del estudio de Campspot, **ningún estado depende únicamente del color**. Cada píldora de estado en tablas y cuadrante combina **fondo suave + icono Unicode discriminable (`✓`, `✕`, `◷`, `⚠`, `✦`) + texto explícito**.
- **Tipografía y adaptación (`TRA-11`):** fuente sans-serif `Segoe UI` (13.5 pt base, rango legible en escritorio, sin abuso de TODO MAYÚSCULAS) y adaptación inmediata en la cabecera mediante `A-` (12 pt), `A` (13.5 pt), `A+` (16 pt) y el botón `Contraste` (modo de alto contraste).

### 7.4 Metáforas, iconos y estilos de interacción (`T4-Metaforas`, `T6-EstilosInt`)
- **Metáforas e iconos coherentes:** `+` para crear, `✎` para editar, `🗑` para eliminar, `?` para ayuda, `◀`/`▶` para navegar en el tiempo. Se acompañan siempre de etiqueta textual y `ToolTip` para evitar ambigüedad.
- **Combinación de estilos de interacción (Shneiderman & Plaisant):**
  - **WIMP y pestañas:** ventanas, iconos, botones y listas seleccionables.
  - **Formularios con guías de diseño (`T6`):** campos obligatorios señalados con `*` rojo, pistas de formato (`HH:mm`), controles específicos (`DatePicker`, `RadioButton` para 3 opciones exclusivas y `CheckBox` para decisiones binarias según guía Microsoft `T3-PEG`), y botones específicos (`Guardar reserva` en lugar de genéricos `OK`).
  - **Manipulación directa ligera y atajos para usuarios frecuentes:** doble clic en filas para editar, clic directo en celdas `✓ Libre` del cuadrante para reservar en esa fecha, y aceleradores de teclado (`F1`, `Ctrl+1..5`, `Ctrl+N`, `Ctrl+E`, `Supr`).

### 7.5 Las 10 Heurísticas de Nielsen y las 8 Reglas de Oro de Shneiderman
1. **Visibilidad del estado del sistema / Feedback:** barra de estado inferior con icono, mensaje y hora tras cada alta, edición, cambio de estado o baja; cálculo en vivo de noches e importe al editar una reserva; usuario activo y último acceso siempre visibles en cabecera.
2. **Lenguaje del usuario y mundo real:** terminología propia de recepción de camping (*parcela, bungalow, estancia mínima, check-in/En curso, pendiente de limpieza*), sin códigos técnicos internos.
3. **Control y libertad del usuario (salidas de emergencia):** botón `Cancelar` (`IsCancel="True"`, tecla `Esc`) en todos los diálogos modales, `Limpiar` en filtros y login, y opción `Cerrar sesión`.
4. **Consistencia y estándares:** misma disposición de botones, estilos `Primary`/`Secondary`/`Danger` y posición fija de la ayuda en todas las ventanas.
5. **Prevención de errores (diferenciando *slips* y *mistakes*):** se previenen *slips* bloqueando alojamientos `En reparación` en el combo de reservas y validando tipos numéricos/horarios; se previenen *mistakes* mediante detección de solapes, aviso automático de conflictos de monitor y **cuadros de confirmación conservadores (`MessageBoxResult.No` predeterminado)** antes de eliminar registros o salir, aplicando el *principio de esfuerzo proporcionado* (`T1`).
6. **Reconocer, diagnosticar y recuperarse de errores:** mensajes inline claros en rojo junto al formulario (en `LoginWindow` y los 4 formularios modales) que indican exactamente qué corregir y sitúan el foco en el control afectado (`control.Focus()`).
7. **Ayuda y documentación:** `ToolTip` en todos los controles interactivos y ventana `AyudaWindow` accesible mediante botón o tecla `F1` con guía por módulos, lista de atajos y autoría.

### 7.6 Evaluación aplicada (DCU / MPIu+a, `T5-Evaluacion Usabilidad`)
Durante el desarrollo se aplicó evaluación **formativa** en dos ciclos:
- **Inspección heurística y recorrido cognitivo** sobre los bocetos iniciales de Balsamiq, que permitió detectar y corregir incoherencias de dominio (tipos de alojamiento, obligatoriedad de teléfono frente a email), ausencia de pestaña de disponibilidad y carencia de ficha Master/Detail en empleados.
- **Inspección de estándares y validación visual** sobre las capturas compiladas de WPF, que permitió detectar y corregir un problema de contraste en el texto de botones primarios y la resolución de rutas `pack://` de recursos embebidos antes de la entrega final.

---

## 8. Manual de usuario (vídeo demostración) y conclusiones

El manual de usuario en formato de vídeo demostración sigue el guion estructurado recogido en `Manual_Usuario_Video_Guion.txt`, diseñado para grabarse con Microsoft Teams recorriendo inicio de sesión, adaptación visual, gestión de alojamientos con galería, cuadrante de disponibilidad, reservas con validaciones, empleados, actividades con detección de conflictos, ayuda (`F1`) y cierre seguro.

**Conclusiones:** el prototipo final satisface íntegramente los requisitos funcionales y transversales definidos en `EnunciadoPractica2627.pdf` y en el Hito 1, materializando de forma verificable los principios teóricos de Interacción Persona-Ordenador I en una aplicación WPF estable, limpia y compilable sin dependencias externas.
