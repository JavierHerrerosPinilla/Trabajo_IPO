using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Trabajo_IPO
{
    public partial class ReservaFormWindow : Window
    {
        private readonly Reserva reservaOriginal;

        public Reserva ReservaGuardada { get; private set; }

        public ReservaFormWindow(Reserva reserva = null, Alojamiento preseleccionado = null, DateTime? fechaPreseleccionada = null)
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
                HoraEntradaTextBox.Text = string.IsNullOrWhiteSpace(reserva.HoraEntradaEstimada) ? "16:00" : reserva.HoraEntradaEstimada;
                HoraSalidaTextBox.Text = string.IsNullOrWhiteSpace(reserva.HoraSalidaEstimada) ? "11:00" : reserva.HoraSalidaEstimada;
                ClienteTextBox.Text = reserva.Cliente;
                TelefonosTextBox.Text = reserva.Telefonos;
                EmailTextBox.Text = reserva.Email;
                OcupantesTextBox.Text = reserva.Ocupantes.ToString(CultureInfo.InvariantCulture);
                CunaCheckBox.IsChecked = reserva.NecesitaCuna;
                MenajeCheckBox.IsChecked = reserva.NecesitaMenaje;
                MovilidadCheckBox.IsChecked = reserva.MovilidadReducida;
                SolicitudesTextBox.Text = reserva.SolicitudesEspeciales;
            }
            else
            {
                if (preseleccionado != null && preseleccionado.Estado != "En reparación")
                {
                    AlojamientoComboBox.SelectedItem = preseleccionado;
                }
                else if (AlojamientoComboBox.Items.Count > 0)
                {
                    AlojamientoComboBox.SelectedIndex = 0;
                }

                if (fechaPreseleccionada.HasValue)
                {
                    EntradaDatePicker.SelectedDate = fechaPreseleccionada.Value;
                    int minN = preseleccionado?.EstanciaMinimaNoches ?? 2;
                    SalidaDatePicker.SelectedDate = fechaPreseleccionada.Value.AddDays(Math.Max(1, minN));
                }
            }

            ActualizarResumenEnVivo();
        }

        private void DatosEstancia_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded) return;
            ErrorBorder.Visibility = Visibility.Collapsed;
            ActualizarResumenEnVivo();
        }

        private void ActualizarResumenEnVivo()
        {
            var alojamiento = AlojamientoComboBox.SelectedItem as Alojamiento;
            if (alojamiento != null)
            {
                string minTxt = alojamiento.Tipo == "Bungalow" && alojamiento.EstanciaMinimaNoches.HasValue
                    ? $" · Estancia mínima: {alojamiento.EstanciaMinimaNoches.Value} noches"
                    : string.Empty;
                CapacidadInfoTextBlock.Text = $"Capacidad máxima: {alojamiento.Capacidad} personas{minTxt}";
            }
            else
            {
                CapacidadInfoTextBlock.Text = string.Empty;
            }

            DateTime? ent = EntradaDatePicker.SelectedDate;
            DateTime? sal = SalidaDatePicker.SelectedDate;
            if (alojamiento != null && ent.HasValue && sal.HasValue && sal.Value.Date > ent.Value.Date)
            {
                int noches = (sal.Value.Date - ent.Value.Date).Days;
                string temporada = DatosPrueba.CalcularTemporada(ent.Value);
                decimal importe = DatosPrueba.CalcularImporteReserva(alojamiento, ent.Value, sal.Value);
                ResumenImporteTextBlock.Text = string.Format(
                    CultureInfo.CurrentCulture,
                    "Resumen estimado: {0} {1} · Temporada {2} · Importe estimado: {3:C}",
                    noches, noches == 1 ? "noche" : "noches", temporada.ToLowerInvariant(), importe);
            }
            else
            {
                ResumenImporteTextBlock.Text = "Seleccione un alojamiento y un rango válido de fechas (salida posterior a entrada).";
            }
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            var alojamiento = AlojamientoComboBox.SelectedItem as Alojamiento;
            DateTime? entrada = EntradaDatePicker.SelectedDate;
            DateTime? salida = SalidaDatePicker.SelectedDate;

            if (string.IsNullOrWhiteSpace(ClienteTextBox.Text))
            {
                MostrarError("El nombre y apellidos de la persona que reserva son obligatorios.", ClienteTextBox);
                return;
            }

            string telefonos = TelefonosTextBox.Text.Trim();
            if (!ValidarTelefonos(telefonos))
            {
                MostrarError("Indique al menos un teléfono válido (entre 9 y 15 dígitos).", TelefonosTextBox);
                return;
            }

            string email = EmailTextBox.Text.Trim();
            if (!string.IsNullOrWhiteSpace(email) && !ValidarEmail(email))
            {
                MostrarError("El formato del correo electrónico no es válido (ej. nombre@dominio.es).", EmailTextBox);
                return;
            }

            if (alojamiento == null)
            {
                MostrarError("Seleccione un alojamiento disponible.", AlojamientoComboBox);
                return;
            }
            if (alojamiento.Estado == "En reparación")
            {
                MostrarError("No se pueden crear reservas en un alojamiento en reparación.", AlojamientoComboBox);
                return;
            }
            if (!entrada.HasValue || !salida.HasValue)
            {
                MostrarError("Debe indicar las fechas de entrada y salida.", EntradaDatePicker);
                return;
            }
            if (salida.Value.Date <= entrada.Value.Date)
            {
                MostrarError("La fecha de salida debe ser posterior a la fecha de entrada.", SalidaDatePicker);
                return;
            }

            int noches = (salida.Value.Date - entrada.Value.Date).Days;
            if (alojamiento.Tipo == "Bungalow" && alojamiento.EstanciaMinimaNoches.HasValue &&
                noches < alojamiento.EstanciaMinimaNoches.Value)
            {
                MostrarError(
                    $"El bungalow {alojamiento.Codigo} exige una estancia mínima de {alojamiento.EstanciaMinimaNoches.Value} noches.",
                    SalidaDatePicker);
                return;
            }

            if (!ValidarHora(HoraEntradaTextBox.Text.Trim()) || !ValidarHora(HoraSalidaTextBox.Text.Trim()))
            {
                MostrarError("Las horas estimadas deben tener formato HH:mm (ej. 16:00).", HoraEntradaTextBox);
                return;
            }

            if (!int.TryParse(OcupantesTextBox.Text.Trim(), out int ocupantes) || ocupantes <= 0)
            {
                MostrarError("El número de ocupantes debe ser un entero mayor que cero.", OcupantesTextBox);
                return;
            }
            if (ocupantes > alojamiento.Capacidad)
            {
                MostrarError($"El número de ocupantes ({ocupantes}) supera la capacidad del alojamiento ({alojamiento.Capacidad}).", OcupantesTextBox);
                return;
            }

            bool solapada = DatosPrueba.Reservas.Any(r =>
                r != reservaOriginal &&
                r.Alojamiento == alojamiento.Codigo &&
                r.Estado != "Cancelada" &&
                r.Estado != "Finalizada" &&
                entrada.Value.Date < r.Salida.Date &&
                salida.Value.Date > r.Entrada.Date);
            if (solapada)
            {
                MostrarError($"El alojamiento {alojamiento.Codigo} ya tiene otra reserva activa que se solapa con esas fechas.", EntradaDatePicker);
                return;
            }

            decimal importe = DatosPrueba.CalcularImporteReserva(alojamiento, entrada.Value, salida.Value);

            Reserva destino = reservaOriginal ?? new Reserva
            {
                Codigo = ObtenerNuevoCodigo(),
                Estado = "Confirmada"
            };

            destino.Alojamiento = alojamiento.Codigo;
            destino.Cliente = ClienteTextBox.Text.Trim();
            destino.Telefonos = telefonos;
            destino.Email = email;
            destino.Entrada = entrada.Value.Date;
            destino.Salida = salida.Value.Date;
            destino.HoraEntradaEstimada = HoraEntradaTextBox.Text.Trim();
            destino.HoraSalidaEstimada = HoraSalidaTextBox.Text.Trim();
            destino.Ocupantes = ocupantes;
            destino.NecesitaCuna = CunaCheckBox.IsChecked == true;
            destino.NecesitaMenaje = MenajeCheckBox.IsChecked == true;
            destino.MovilidadReducida = MovilidadCheckBox.IsChecked == true;
            destino.SolicitudesEspeciales = SolicitudesTextBox.Text.Trim();
            destino.ImporteEstimado = importe;
            destino.RefrescarPropiedadesCalculadas();

            ReservaGuardada = destino;
            DialogResult = true;
        }

        private static bool ValidarTelefonos(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return false;
            var partes = raw.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (partes.Length == 0) return false;
            foreach (var p in partes)
            {
                string digits = Regex.Replace(p, @"[^\d]", "");
                if (digits.Length < 9 || digits.Length > 15) return false;
            }
            return true;
        }

        private static bool ValidarEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private static bool ValidarHora(string hora)
        {
            if (string.IsNullOrWhiteSpace(hora)) return true;
            return TimeSpan.TryParseExact(hora, @"hh\:mm", CultureInfo.InvariantCulture, out _);
        }

        private string ObtenerNuevoCodigo()
        {
            int siguiente = DatosPrueba.Reservas
                .Select(r => r.Codigo)
                .Select(c => c != null && c.StartsWith("R-") ? c.Substring(2) : "0")
                .Select(c => int.TryParse(c, out int n) ? n : 0)
                .DefaultIfEmpty(1000)
                .Max() + 1;
            return "R-" + siguiente.ToString(CultureInfo.InvariantCulture);
        }

        private void MostrarError(string mensaje, Control control)
        {
            ErrorTextBlock.Text = "✕ " + mensaje;
            ErrorBorder.Visibility = Visibility.Visible;
            control.Focus();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Ayuda_Click(object sender, RoutedEventArgs e)
        {
            new AyudaWindow("Reservas") { Owner = this }.ShowDialog();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                Ayuda_Click(sender, e);
                e.Handled = true;
            }
        }
    }
}
