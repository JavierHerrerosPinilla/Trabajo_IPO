using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Trabajo_IPO
{
    public partial class AyudaWindow : Window
    {
        private readonly Dictionary<string, string> textosAyuda = new Dictionary<string, string>
        {
            ["Primeros pasos"] =
                "1. Inicie sesión con su identificador y contraseña (ej. admin / camping2026).\n" +
                "2. Seleccione el área de trabajo en las pestañas superiores: Alojamientos, Disponibilidad, Reservas, Empleados o Actividades.\n" +
                "3. Utilice los botones '+ Nuevo', 'Editar' y 'Eliminar' de cada pestaña para gestionar la información de forma dinámica.\n" +
                "4. Pase el puntero sobre cualquier botón, campo o badge de estado para consultar su ayuda contextual (tooltip).\n" +
                "5. Pulse F1 en cualquier ventana para abrir esta ayuda.\n\n" +
                "Nota: los datos se mantienen en memoria durante la sesión y no requieren base de datos externa.",

            ["Alojamientos"] =
                "• Permite registrar, modificar, consultar y eliminar parcelas y bungalows.\n" +
                "• Parcelas: incluyen categoría (Pequeña, Mediana, Deluxe, Autocaravana), tamaño (m²), ubicación, capacidad, tarifas diferenciadas por temporada (baja, media y alta) y servicios (agua, luz, desagüe, setos, sombra).\n" +
                "• Bungalows: incluyen tamaño, capacidad máxima, tarifa por noche, estancia mínima opcional, equipamiento (menaje, microondas, ropa de cama y toallas, Wi-Fi, terraza, aire acondicionado), descripción y galería de fotos.\n" +
                "• Al dar de baja un alojamiento con reservas activas o futuras, el sistema advierte explícitamente antes de confirmar.",

            ["Disponibilidad"] =
                "• Muestra un cuadrante tipo calendario con una fila por alojamiento y columnas por días (navegación por quincenas).\n" +
                "• Cada estado se representa de forma accesible combinando color, icono y texto: Libre, Ocupada, Reservada, En reparación y Pendiente de limpieza.\n" +
                "• Al hacer clic sobre una celda Libre puede iniciar directamente una reserva preseleccionada; también puede cambiar manualmente el estado de cualquier alojamiento.",

            ["Reservas"] =
                "• Permite crear, editar, consultar y cancelar reservas válidas.\n" +
                "• Valida automáticamente: formato de teléfono (obligatorio) y correo electrónico (opcional), fecha de salida posterior a la de entrada, capacidad máxima del alojamiento, estancia mínima en bungalows, bloqueo de alojamientos en reparación y detección de solapamientos.\n" +
                "• Calcula en tiempo real el número de noches, la temporada aplicable y el importe estimado.",

            ["Empleados"] =
                "• Gestiona los guías y monitores temporales contratados para la temporada de verano.\n" +
                "• Permite almacenar foto reciente, nombre, apellidos, función, teléfonos, correos electrónicos, idiomas, formación y restricciones de disponibilidad por días de la semana.\n" +
                "• La ficha lateral muestra automáticamente las actividades asignadas a cada empleado y avisa al darlo de baja si tiene actividades vinculadas.",

            ["Actividades"] =
                "• Gestiona las actividades de verano (nombre, descripción, área, destinatarios, días, horario, cupo mínimo y máximo, precio/hora y/o precio/mes, materiales y monitores responsables).\n" +
                "• El sistema avisa automáticamente si un monitor asignado tiene otra actividad solapada o días no disponibles según su ficha.",

            ["Atajos y accesibilidad"] =
                "Atajos de teclado disponibles en la ventana principal:\n" +
                "• F1: Abrir ayuda y autoría\n" +
                "• Ctrl + 1..5: Cambiar entre las 5 pestañas principales\n" +
                "• Ctrl + N: Crear nuevo elemento en la pestaña actual\n" +
                "• Ctrl + E: Editar el elemento seleccionado\n" +
                "• Supr: Eliminar / cancelar el elemento seleccionado (pide confirmación)\n\n" +
                "Adaptación de interfaz:\n" +
                "• Controles A- / A / A+ en la cabecera para ajustar el tamaño de fuente.\n" +
                "• Botón 'Contraste' para activar el modo de alto contraste.",

            ["Acerca de"] =
                "Prototipo de aplicación de escritorio desarrollado en C# y WPF (.NET Framework 4.7.2) aplicando principios de Interacción Persona-Ordenador: leyes de Gestalt, reducción de carga cognitiva (7±2), reconocimiento frente a recuerdo, codificación redundante de estados, prevención de errores, salidas de emergencia y ayuda contextual."
        };

        public AyudaWindow(string temaInicial = null)
        {
            InitializeComponent();
            ActualizarContenido("Primeros pasos");

            if (!string.IsNullOrWhiteSpace(temaInicial))
            {
                foreach (ListBoxItem item in TemasListBox.Items)
                {
                    if ((item.Content as string) == temaInicial)
                    {
                        item.IsSelected = true;
                        break;
                    }
                }
            }
        }

        private void TemasListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TemasListBox.SelectedItem is ListBoxItem item && item.Content is string tema)
            {
                ActualizarContenido(tema);
            }
        }

        private void ActualizarContenido(string tema)
        {
            if (ContenidoGroupBox == null || ContenidoTextBlock == null) return;
            ContenidoGroupBox.Header = tema;
            ContenidoTextBlock.Text = textosAyuda.ContainsKey(tema) ? textosAyuda[tema] : string.Empty;
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
                e.Handled = true;
            }
        }
    }
}
