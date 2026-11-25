using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Trabajo_IPO
{
    public partial class ActividadFormWindow : Window
    {
        public class MonitorSelectable
        {
            public string Nombre { get; set; }
            public bool Seleccionado { get; set; }
        }

        private readonly Actividad actividadOriginal;
        private readonly ObservableCollection<MonitorSelectable> monitoresLista = new ObservableCollection<MonitorSelectable>();

        public Actividad ActividadGuardada { get; private set; }

        public ActividadFormWindow(Actividad actividad = null)
        {
            InitializeComponent();
            actividadOriginal = actividad;
            TituloTextBlock.Text = actividad == null ? "Nueva actividad de verano" : "Modificar actividad de verano";

            var asignados = (actividad?.Monitor ?? string.Empty)
                .Split(',')
                .Select(m => m.Trim())
                .Where(m => m.Length > 0)
                .ToList();

            foreach (var emp in DatosPrueba.Empleados)
            {
                monitoresLista.Add(new MonitorSelectable
                {
                    Nombre = emp.NombreCompleto,
                    Seleccionado = asignados.Any(a => a.Equals(emp.NombreCompleto, StringComparison.OrdinalIgnoreCase))
                });
            }
            MonitoresItemsControl.ItemsSource = monitoresLista;

            if (actividad != null)
            {
                NombreTextBox.Text = actividad.Nombre;
                ZonaComboBox.Text = actividad.Zona;
                DescripcionTextBox.Text = actividad.Descripcion;
                HoraInicioTextBox.Text = actividad.HoraInicio;
                HoraFinTextBox.Text = actividad.HoraFin;
                CupoMinTextBox.Text = actividad.CupoMinimo.ToString(CultureInfo.InvariantCulture);
                CupoMaxTextBox.Text = actividad.CupoMaximo.ToString(CultureInfo.InvariantCulture);
                PrecioHoraTextBox.Text = actividad.PrecioHora.HasValue
                    ? actividad.PrecioHora.Value.ToString("0.##", CultureInfo.CurrentCulture) : string.Empty;
                PrecioMesTextBox.Text = actividad.PrecioMes.HasValue
                    ? actividad.PrecioMes.Value.ToString("0.##", CultureInfo.CurrentCulture) : string.Empty;
                MaterialesTextBox.Text = actividad.Materiales;

                if (actividad.Destinatarios == "Niños") DestNiñosRadio.IsChecked = true;
                else if (actividad.Destinatarios == "Adultos") DestAdultosRadio.IsChecked = true;
                else DestTodosRadio.IsChecked = true;

                var dias = (actividad.DiasSemana ?? string.Empty).Split(',').Select(d => d.Trim()).ToList();
                DiaLunCheckBox.IsChecked = dias.Contains("Lun");
                DiaMarCheckBox.IsChecked = dias.Contains("Mar");
                DiaMieCheckBox.IsChecked = dias.Contains("Mié");
                DiaJueCheckBox.IsChecked = dias.Contains("Jue");
                DiaVieCheckBox.IsChecked = dias.Contains("Vie");
                DiaSabCheckBox.IsChecked = dias.Contains("Sáb");
                DiaDomCheckBox.IsChecked = dias.Contains("Dom");
            }
            else
            {
                ZonaComboBox.SelectedIndex = 0;
                DiaLunCheckBox.IsChecked = true;
                DiaMieCheckBox.IsChecked = true;
                DiaVieCheckBox.IsChecked = true;
            }

            ComprobarConflictosEnVivo();
        }

        private string ObtenerDiasSeleccionados()
        {
            var dias = new List<string>();
            if (DiaLunCheckBox.IsChecked == true) dias.Add("Lun");
            if (DiaMarCheckBox.IsChecked == true) dias.Add("Mar");
            if (DiaMieCheckBox.IsChecked == true) dias.Add("Mié");
            if (DiaJueCheckBox.IsChecked == true) dias.Add("Jue");
            if (DiaVieCheckBox.IsChecked == true) dias.Add("Vie");
            if (DiaSabCheckBox.IsChecked == true) dias.Add("Sáb");
            if (DiaDomCheckBox.IsChecked == true) dias.Add("Dom");
            return string.Join(", ", dias);
        }

        private void HorarioOMonitor_Changed(object sender, TextChangedEventArgs e)
        {
            if (IsLoaded) ComprobarConflictosEnVivo();
        }

        private void Dia_Changed(object sender, RoutedEventArgs e)
        {
            if (IsLoaded) ComprobarConflictosEnVivo();
        }

        private void MonitorCheck_Changed(object sender, RoutedEventArgs e)
        {
            if (IsLoaded) ComprobarConflictosEnVivo();
        }

        private void ComprobarConflictosEnVivo()
        {
            if (AvisoMonitorBorder == null) return;
            string dias = ObtenerDiasSeleccionados();
            string ini = HoraInicioTextBox.Text.Trim();
            string fin = HoraFinTextBox.Text.Trim();
            var avisos = new List<string>();

            foreach (var m in monitoresLista.Where(x => x.Seleccionado))
            {
                avisos.AddRange(DatosPrueba.DetectarConflictosMonitor(m.Nombre, dias, ini, fin, actividadOriginal));
            }

            if (avisos.Count > 0)
            {
                AvisoMonitorTextBlock.Text = "⚠ Aviso de disponibilidad/solape: " + string.Join(" | ", avisos);
                AvisoMonitorBorder.Visibility = Visibility.Visible;
            }
            else
            {
                AvisoMonitorBorder.Visibility = Visibility.Collapsed;
            }
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            string nombre = NombreTextBox.Text.Trim();
            string zona = ZonaComboBox.Text.Trim();
            string dias = ObtenerDiasSeleccionados();
            string iniTxt = HoraInicioTextBox.Text.Trim();
            string finTxt = HoraFinTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                MostrarError("El nombre de la actividad es obligatorio.", NombreTextBox);
                return;
            }
            if (string.IsNullOrWhiteSpace(zona))
            {
                MostrarError("Indique el área del camping donde se desarrolla.", ZonaComboBox);
                return;
            }
            if (!TimeSpan.TryParseExact(iniTxt, @"hh\:mm", CultureInfo.InvariantCulture, out TimeSpan ini) ||
                !TimeSpan.TryParseExact(finTxt, @"hh\:mm", CultureInfo.InvariantCulture, out TimeSpan fin) ||
                fin <= ini)
            {
                MostrarError("Indique un horario válido (HH:mm) con hora de fin posterior a la de inicio.", HoraInicioTextBox);
                return;
            }
            if (string.IsNullOrWhiteSpace(dias))
            {
                MostrarError("Seleccione al menos un día de la semana.", DiaLunCheckBox);
                return;
            }
            if (!int.TryParse(CupoMinTextBox.Text.Trim(), out int cupoMin) || cupoMin <= 0)
            {
                MostrarError("El cupo mínimo debe ser un entero positivo.", CupoMinTextBox);
                return;
            }
            if (!int.TryParse(CupoMaxTextBox.Text.Trim(), out int cupoMax) || cupoMax < cupoMin)
            {
                MostrarError("El cupo máximo debe ser mayor o igual que el cupo mínimo.", CupoMaxTextBox);
                return;
            }

            decimal? pHora = null, pMes = null;
            if (!string.IsNullOrWhiteSpace(PrecioHoraTextBox.Text))
            {
                if (!TryParseDecimalFlexible(PrecioHoraTextBox.Text, out decimal ph) || ph <= 0)
                {
                    MostrarError("El precio por hora debe ser un número positivo.", PrecioHoraTextBox);
                    return;
                }
                pHora = ph;
            }
            if (!string.IsNullOrWhiteSpace(PrecioMesTextBox.Text))
            {
                if (!TryParseDecimalFlexible(PrecioMesTextBox.Text, out decimal pm) || pm <= 0)
                {
                    MostrarError("El precio por mes debe ser un número positivo.", PrecioMesTextBox);
                    return;
                }
                pMes = pm;
            }
            if (!pHora.HasValue && !pMes.HasValue)
            {
                MostrarError("Indique al menos un precio (por hora y/o por mes).", PrecioHoraTextBox);
                return;
            }

            var monitores = monitoresLista.Where(m => m.Seleccionado).Select(m => m.Nombre).ToList();
            if (monitores.Count == 0)
            {
                MostrarError("Asigne al menos un monitor o guía responsable.", MonitoresItemsControl);
                return;
            }

            // Confirmar explícitamente si hay avisos de conflicto o restricción (ACT-12)
            if (AvisoMonitorBorder.Visibility == Visibility.Visible)
            {
                var resp = MessageBox.Show(
                    AvisoMonitorTextBlock.Text + "\n\n¿Desea guardar la actividad igualmente?",
                    "Conflicto de monitor detectado",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning,
                    MessageBoxResult.No);
                if (resp != MessageBoxResult.Yes) return;
            }

            string destinatarios = DestNiñosRadio.IsChecked == true ? "Niños"
                : (DestAdultosRadio.IsChecked == true ? "Adultos" : "Todos");

            Actividad destino = actividadOriginal ?? new Actividad();
            destino.Nombre = nombre;
            destino.Zona = zona;
            destino.Destinatarios = destinatarios;
            destino.Descripcion = DescripcionTextBox.Text.Trim();
            destino.DiasSemana = dias;
            destino.HoraInicio = iniTxt;
            destino.HoraFin = finTxt;
            destino.CupoMinimo = cupoMin;
            destino.CupoMaximo = cupoMax;
            destino.PrecioHora = pHora;
            destino.PrecioMes = pMes;
            destino.Monitor = string.Join(", ", monitores);
            destino.Materiales = MaterialesTextBox.Text.Trim();
            destino.RefrescarPropiedadesCalculadas();

            ActividadGuardada = destino;
            DialogResult = true;
        }

        private static bool TryParseDecimalFlexible(string raw, out decimal val)
        {
            string norm = (raw ?? string.Empty).Trim().Replace(',', '.');
            return decimal.TryParse(norm, NumberStyles.Number, CultureInfo.InvariantCulture, out val);
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
            new AyudaWindow("Actividades") { Owner = this }.ShowDialog();
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
