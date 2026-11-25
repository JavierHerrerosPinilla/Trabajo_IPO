# Lista de pendientes — Prototipo WPF Gestión de Complejo Turístico (IPO I)

Análisis comparativo entre el estado inicial del proyecto, los requisitos de `EnunciadoPractica2627.pdf`, las áreas funcionales de `Hito1_IPO_Gestion_Complejo_final_Entrega.pdf` (secciones 1–4, excluyendo bocetos del punto 5), los bocetos mejorados en `Bocetos/New Project copy.bmpr` y la teoría de `Teoría/`.

**Estado final:** Todos los elementos han sido implementados y verificados mediante compilación (`Debug` y `Release`, 0 errores) y revisión visual de todas las pantallas.

---

## 1. Autenticación y acceso (`AUT`)
- [x] **AUT-01 / AUT-02**: Validar identificador y clave en `LoginWindow` mostrando el error **inline junto al formulario** (visión periférica y heurística 9 de Nielsen), sin depender de `MessageBox` modales intrusivos; añadida opción «Mostrar contraseña».
- [x] **AUT-03 / AUT-04**: Mantener varios gestores con **foto de perfil real embebida**, rol y fecha/hora de último acceso visible en el encabezado de `MainWindow`, actualizando el último acceso en sesión.
- [x] **AUT-05**: Permitir **Cerrar sesión** desde `MainWindow` para volver a `LoginWindow` previa confirmación.
- [x] **AUT-06**: Permitir **Salir** de la aplicación en cualquier momento (tanto en `LoginWindow` como en `MainWindow` y al cerrar con la `X`), pidiendo confirmación explícita con «No» como opción predeterminada.

## 2. Alojamientos (`ALO`)
- [x] **ALO-01 a ALO-05 (Parcelas)**: Añadidos `Categoria` (Pequeña, Mediana, Deluxe, Autocaravana), `TamanoM2`, `Ubicacion` estructurada, **tarifas por temporada** (`PrecioTempBaja`, `PrecioTempMedia`, `PrecioTempAlta`) y flags completos de servicios (`TieneAgua`, `TieneLuz`, `TieneDesague`, `DelimitadaSetos`, `ZonaSombra`).
- [x] **ALO-06 a ALO-09 (Bungalows)**: Añadidos `TamanoM2`, `EstanciaMinimaNoches` (opcional), equipamiento completo (`TieneMenaje`, `TieneMicroondas`, `TieneRopaCamaToallas`, `TieneWifi`, `TieneTerraza`, `TieneAireAcondicionado`), `Descripcion` breve y **galería de fotos** (añadir, previsualizar y quitar fotos embebidas o locales).
- [x] **ALO-10 a ALO-15**: Alta, modificación, consulta con **ficha Master/Detail** inferior, filtros combinables (tipo, capacidad mínima, estado, búsqueda texto), cambio manual de estado, prevención de duplicados/valores inválidos y **baja con confirmación avisando si existen reservas futuras**.

## 3. Disponibilidad (`DIS`)
- [x] **DIS-01 / DIS-02**: Mantenimiento de los 5 estados (`Libre`, `Ocupada`, `Reservada`, `En reparación`, `Pendiente de limpieza`) con cambio manual.
- [x] **DIS-03**: Implementada pestaña **Disponibilidad** en `MainWindow` con cuadrante tipo calendario por quincenas (`◀ Anterior` / `Siguiente ▶`), una fila por alojamiento y celdas clicables (creación directa de reserva en días libres).
- [x] **DIS-04**: Representación accesible de cada estado con **color + icono + texto** mediante convertidores dedicados.
- [x] **DIS-05**: Filtros por tipo de alojamiento y estado en la vista de Disponibilidad + leyenda accesible visible.

## 4. Reservas (`RES`)
- [x] **RES-01 a RES-07**: Añadidos a `Reserva` teléfonos de contacto (obligatorio), correo electrónico (opcional), horas estimadas de entrada y salida, solicitudes especiales estructuradas (`NecesitaCuna`, `NecesitaMenaje`, `MovilidadReducida` + observaciones) y cálculo de noches e importe estimado según temporada.
- [x] **RES-08 a RES-11**: Listado con columnas completas, filtros por rango de fechas, alojamiento y cliente, más panel **Master/Detail** con cambio rápido de estado (`En curso`, `Finalizada`).
- [x] **RES-12 a RES-18**: Validación de fechas (`Salida > Entrada`), capacidad máxima, anti-solapamiento, bloqueo de alojamientos `En reparación`, validación de formato de teléfono/email, respeto a la **estancia mínima** en bungalows y resumen en vivo de noches, temporada e importe estimado.

## 5. Empleados temporales: guías y monitores (`EMP`)
- [x] **EMP-01 a EMP-04**: Separados `Nombre` y `Apellidos`, foto reciente (con selector de imagen), múltiples teléfonos y correos, idiomas, formación y restricciones de disponibilidad por días de la semana (`Lun..Dom`) + notas.
- [x] **EMP-05 / EMP-06**: Implementado `EmpleadoFormWindow` completo para **alta y modificación**, y habilitada **baja** con confirmación avisando si el empleado tiene actividades asignadas.
- [x] **EMP-07 a EMP-09**: Vista **Master/Detail horizontal** en la pestaña Empleados (tabla + ficha lateral con foto y actividades asignadas), filtros combinables (función, idioma, búsqueda) y validación inline.

## 6. Actividades de verano (`ACT`)
- [x] **ACT-01 a ACT-06**: Ampliada `Actividad` con descripción, días de la semana, hora inicio/fin, múltiples monitores responsables, `CupoMinimo` y `CupoMaximo`, `Destinatarios`, `PrecioHora` y/o `PrecioMes`, área del camping y materiales/equipamiento.
- [x] **ACT-07 / ACT-08**: Implementado `ActividadFormWindow` completo para **alta y modificación**, y habilitada **baja** previa confirmación.
- [x] **ACT-09 a ACT-12**: Ficha **Master/Detail** en la pestaña Actividades, filtros por área, destinatarios, día y búsqueda, validación (`CupoMaximo >= CupoMinimo`, al menos un precio, horario válido) y **aviso automático en vivo si un monitor tiene solape horario o restricción de disponibilidad**.

## 7. Requisitos transversales (`TRA`) y Teoría IPO
- [x] **TRA-01 a TRA-03**: Ventana principal única con **5 pestañas** (Miller 7±2) y actualización dinámica de listados (`NotifiableBase` + `ObservableCollection`).
- [x] **TRA-04 / TRA-05 / TRA-07**: Ayuda accesible en todo momento (botón visible + tecla `F1`), `ToolTip` descriptivos en todos los controles y `AyudaWindow` con índice lateral de 8 temas e información de autoría real (**Javier Herreros Pinilla** y **Miguel Gómez-Carreño Rodríguez-Escalona**).
- [x] **TRA-06 / TRA-08 / TRA-09**: Confirmaciones conservadoras (`No` por defecto), campos obligatorios con `*`, validación inline junto al campo afectado y botón `Cancelar` en todos los formularios.
- [x] **TRA-10**: Barra de estado dinámica en `MainWindow` informando del resultado de cada operación (icono + mensaje + hora) y atajos activos.
- [x] **TRA-11 / TRA-12**: Adaptación de interfaz accesible (`A-` / `A` / `A+` + modo **Contraste**) y ventanas redimensionables sin pérdida de legibilidad.
- [x] **TRA-13**: Datos de prueba realistas y recursos gráficos embebidos en `Assets/` (`Resource` + copia a salida).

## 8. Limpieza, documentación y entregables finales
- [x] Eliminados archivos duplicados, temporales y borradores innecesarios de la raíz del proyecto.
- [x] Actualizar `leeme.txt`, `Manual_Usuario_Video_Guion.txt`, `Memoria_Final.md`, `Memoria_Final.pdf` y `README.md`.
- [x] Compilación final `Debug` y `Release` tras la limpieza y documentación.
