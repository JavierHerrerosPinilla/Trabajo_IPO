using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;

namespace Trabajo_IPO
{
    public partial class EmpleadoFormWindow : Window
    {
        private readonly EmpleadoTemporal empleadoOriginal;
        private string rutaFotoActual;

        public EmpleadoTemporal EmpleadoGuardado { get; private set; }

        public EmpleadoFormWindow(EmpleadoTemporal empleado = null)
        {
            InitializeComponent();
            empleadoOriginal = empleado;
            TituloTextBlock.Text = empleado == null ? "Nuevo empleado temporal" : "Modificar empleado temporal";

            if (empleado != null)
            {
                NombreTextBox.Text = empleado.Nombre;
                ApellidosTextBox.Text = empleado.Apellidos;
                SeleccionarCombo(empleado.Tipo);
                TelefonoTextBox.Text = empleado.Telefono;
                EmailTextBox.Text = empleado.Email;
                FormacionTextBox.Text = empleado.Formacion;
                ObsDisponibilidadTextBox.Text = empleado.ObservacionesDisponibilidad;
                rutaFotoActual = empleado.Foto;

                var idiomas = (empleado.Idiomas ?? string.Empty).Split(',').Select(i => i.Trim()).Where(i => i.Length > 0).ToList();
                EspanolCheckBox.IsChecked = idiomas.Any(i => i.Equals("Español", StringComparison.OrdinalIgnoreCase));
                InglesCheckBox.IsChecked = idiomas.Any(i => i.Equals("Inglés", StringComparison.OrdinalIgnoreCase));
                FrancesCheckBox.IsChecked = idiomas.Any(i => i.Equals("Francés", StringComparison.OrdinalIgnoreCase));
                AlemanCheckBox.IsChecked = idiomas.Any(i => i.Equals("Alemán", StringComparison.OrdinalIgnoreCase));
                var otros = idiomas.Where(i =>
                    !i.Equals("Español", StringComparison.OrdinalIgnoreCase) &&
                    !i.Equals("Inglés", StringComparison.OrdinalIgnoreCase) &&
                    !i.Equals("Francés", StringComparison.OrdinalIgnoreCase) &&
                    !i.Equals("Alemán", StringComparison.OrdinalIgnoreCase)).ToList();
                OtrosIdiomasTextBox.Text = string.Join(", ", otros);

                var noDias = empleado.ListaDiasNoDisponibles;
                NoLunCheckBox.IsChecked = noDias.Contains("Lun");
                NoMarCheckBox.IsChecked = noDias.Contains("Mar");
                NoMieCheckBox.IsChecked = noDias.Contains("Mié");
                NoJueCheckBox.IsChecked = noDias.Contains("Jue");
                NoVieCheckBox.IsChecked = noDias.Contains("Vie");
                NoSabCheckBox.IsChecked = noDias.Contains("Sáb");
                NoDomCheckBox.IsChecked = noDias.Contains("Dom");
            }
            else
            {
                TipoComboBox.SelectedIndex = 0;
                rutaFotoActual = "pack://application:,,,/Assets/emp_eva.png";
            }

            ActualizarPreviewFoto();
        }

        private void SeleccionarCombo(string tipo)
        {
            foreach (ComboBoxItem item in TipoComboBox.Items)
            {
                if (string.Equals(item.Content as string, tipo, StringComparison.OrdinalIgnoreCase))
                {
                    TipoComboBox.SelectedItem = item;
                    return;
                }
            }
            TipoComboBox.SelectedIndex = 0;
        }

        private void ActualizarPreviewFoto()
        {
            var conv = new ImagePathConverter();
            FotoImage.Source = conv.Convert(rutaFotoActual, typeof(ImageSource), null, System.Globalization.CultureInfo.CurrentCulture) as ImageSource;
        }

        private void CambiarFoto_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Title = "Seleccionar fotografía reciente del empleado",
                Filter = "Imágenes|*.png;*.jpg;*.jpeg;*.bmp|Todos los archivos|*.*"
            };
            if (dlg.ShowDialog() == true)
            {
                rutaFotoActual = dlg.FileName;
                ActualizarPreviewFoto();
            }
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            string nombre = NombreTextBox.Text.Trim();
            string apellidos = ApellidosTextBox.Text.Trim();
            string funcion = (TipoComboBox.SelectedItem as ComboBoxItem)?.Content as string ?? "Monitor";
            string telefonos = TelefonoTextBox.Text.Trim();
            string correos = EmailTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                MostrarError("El nombre del empleado es obligatorio.", NombreTextBox);
                return;
            }
            if (string.IsNullOrWhiteSpace(apellidos))
            {
                MostrarError("Los apellidos del empleado son obligatorios.", ApellidosTextBox);
                return;
            }
            if (!ValidarTelefonos(telefonos))
            {
                MostrarError("Indique al menos un teléfono válido (entre 9 y 15 dígitos).", TelefonoTextBox);
                return;
            }
            if (!ValidarEmails(correos))
            {
                MostrarError("Indique al menos un correo electrónico válido.", EmailTextBox);
                return;
            }

            var idiomas = new List<string>();
            if (EspanolCheckBox.IsChecked == true) idiomas.Add("Español");
            if (InglesCheckBox.IsChecked == true) idiomas.Add("Inglés");
            if (FrancesCheckBox.IsChecked == true) idiomas.Add("Francés");
            if (AlemanCheckBox.IsChecked == true) idiomas.Add("Alemán");
            if (!string.IsNullOrWhiteSpace(OtrosIdiomasTextBox.Text))
            {
                idiomas.AddRange(OtrosIdiomasTextBox.Text.Split(',').Select(s => s.Trim()).Where(s => s.Length > 0));
            }

            var noDias = new List<string>();
            if (NoLunCheckBox.IsChecked == true) noDias.Add("Lun");
            if (NoMarCheckBox.IsChecked == true) noDias.Add("Mar");
            if (NoMieCheckBox.IsChecked == true) noDias.Add("Mié");
            if (NoJueCheckBox.IsChecked == true) noDias.Add("Jue");
            if (NoVieCheckBox.IsChecked == true) noDias.Add("Vie");
            if (NoSabCheckBox.IsChecked == true) noDias.Add("Sáb");
            if (NoDomCheckBox.IsChecked == true) noDias.Add("Dom");

            EmpleadoTemporal destino = empleadoOriginal ?? new EmpleadoTemporal();
            destino.Nombre = nombre;
            destino.Apellidos = apellidos;
            destino.Tipo = funcion;
            destino.Telefono = telefonos;
            destino.Email = correos;
            destino.Idiomas = idiomas.Count > 0 ? string.Join(", ", idiomas) : "Español";
            destino.Formacion = FormacionTextBox.Text.Trim();
            destino.DiasNoDisponibles = string.Join(", ", noDias);
            destino.ObservacionesDisponibilidad = ObsDisponibilidadTextBox.Text.Trim();
            destino.Foto = rutaFotoActual;
            destino.RefrescarPropiedadesCalculadas();

            EmpleadoGuardado = destino;
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

        private static bool ValidarEmails(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return false;
            var partes = raw.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (partes.Length == 0) return false;
            foreach (var p in partes)
            {
                if (!Regex.IsMatch(p.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) return false;
            }
            return true;
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
            new AyudaWindow("Empleados") { Owner = this }.ShowDialog();
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
