using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Trabajo_IPO
{
    public partial class ReservaFormWindow : Window
    {
        private readonly Reserva reservaOriginal;

        public Reserva ReservaGuardada { get; private set; }

        public ReservaFormWindow(Reserva reserva = null)
        {
            InitializeComponent();
            reservaOriginal = reserva;
            TituloTextBlock.Text = reserva == null ? "Nueva reserva" : "Modificar reserva";
            AlojamientoComboBox.ItemsSource = DatosPrueba.Alojamientos
                .Where(a => a.Estado != "En reparación" && a.Estado != "Fuera de servicio")
                .ToList();

            if (reserva != null)
            {
                AlojamientoComboBox.SelectedItem = DatosPrueba.Alojamientos
                    .FirstOrDefault(a => a.Codigo == reserva.Alojamiento);
                EntradaDatePicker.SelectedDate = reserva.Entrada;
                SalidaDatePicker.SelectedDate = reserva.Salida;
                ClienteTextBox.Text = reserva.Cliente;
                OcupantesTextBox.Text = reserva.Ocupantes.ToString(CultureInfo.InvariantCulture);
            }
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            Alojamiento alojamiento = AlojamientoComboBox.SelectedItem as Alojamiento;
            DateTime? entrada = EntradaDatePicker.SelectedDate;
            DateTime? salida = SalidaDatePicker.SelectedDate;
            int ocupantes;

            if (alojamiento == null)
            {
                MostrarError("Seleccione un alojamiento.", AlojamientoComboBox);
                return;
            }
            if (!entrada.HasValue || !salida.HasValue)
            {
                MessageBox.Show("Debe indicar las fechas de entrada y salida.",
                    "Revisar datos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (salida.Value <= entrada.Value)
            {
                MessageBox.Show(
                    "La fecha de salida debe ser posterior a la fecha de entrada.",
                    "Fechas no válidas", MessageBoxButton.OK, MessageBoxImage.Error);
                SalidaDatePicker.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(ClienteTextBox.Text))
            {
                MostrarError("El nombre de la persona que reserva es obligatorio.", ClienteTextBox);
                return;
            }
            if (!int.TryParse(OcupantesTextBox.Text.Trim(), out ocupantes) || ocupantes <= 0)
            {
                MostrarError("El número de ocupantes debe ser un entero mayor que cero.",
                    OcupantesTextBox);
                return;
            }
            if (ocupantes > alojamiento.Capacidad)
            {
                MostrarError("El número de ocupantes supera la capacidad del alojamiento.",
                    OcupantesTextBox);
                return;
            }

            bool solapada = DatosPrueba.Reservas.Any(r =>
                r != reservaOriginal &&
                r.Alojamiento == alojamiento.Codigo &&
                entrada.Value < r.Salida &&
                salida.Value > r.Entrada &&
                r.Estado != "Cancelada");
            if (solapada)
            {
                MessageBox.Show("El alojamiento ya tiene una reserva para esas fechas.",
                    "Alojamiento no disponible", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (reservaOriginal == null)
            {
                ReservaGuardada = new Reserva
                {
                    Codigo = ObtenerNuevoCodigo(),
                    Alojamiento = alojamiento.Codigo,
                    Cliente = ClienteTextBox.Text.Trim(),
                    Entrada = entrada.Value,
                    Salida = salida.Value,
                    Ocupantes = ocupantes,
                    Estado = "Pendiente",
                    SolicitudesEspeciales = string.Empty
                };
            }
            else
            {
                reservaOriginal.Alojamiento = alojamiento.Codigo;
                reservaOriginal.Cliente = ClienteTextBox.Text.Trim();
                reservaOriginal.Entrada = entrada.Value;
                reservaOriginal.Salida = salida.Value;
                reservaOriginal.Ocupantes = ocupantes;
                ReservaGuardada = reservaOriginal;
            }

            DialogResult = true;
        }

        private string ObtenerNuevoCodigo()
        {
            int siguiente = DatosPrueba.Reservas
                .Select(r => r.Codigo)
                .Select(c => c.StartsWith("R-") ? c.Substring(2) : "0")
                .Select(c =>
                {
                    int numero;
                    return int.TryParse(c, out numero) ? numero : 0;
                })
                .DefaultIfEmpty(1000)
                .Max() + 1;

            return "R-" + siguiente.ToString(CultureInfo.InvariantCulture);
        }

        private void MostrarError(string mensaje, Control control)
        {
            MessageBox.Show(mensaje, "Revisar datos",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            control.Focus();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
