using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;

namespace Trabajo_IPO
{
    public partial class AlojamientoFormWindow : Window
    {
        private readonly Alojamiento alojamientoOriginal;
        private readonly ObservableCollection<string> fotosEditadas = new ObservableCollection<string>();

        public Alojamiento AlojamientoGuardado { get; private set; }

        public AlojamientoFormWindow(Alojamiento alojamiento = null)
        {
            InitializeComponent();
            alojamientoOriginal = alojamiento;
            TituloTextBlock.Text = alojamiento == null ? "Nuevo alojamiento" : "Modificar alojamiento";

            FotosListBox.ItemsSource = fotosEditadas;

            if (alojamiento != null)
            {
                CodigoTextBox.Text = alojamiento.Codigo;
                TamanoTextBox.Text = alojamiento.TamanoM2.ToString(CultureInfo.CurrentCulture);
                CapacidadTextBox.Text = alojamiento.Capacidad.ToString(CultureInfo.InvariantCulture);
                UbicacionComboBox.Text = alojamiento.Ubicacion;
                SeleccionarComboPorTexto(EstadoComboBox, alojamiento.Estado);
                DescripcionTextBox.Text = alojamiento.Descripcion;

                bool esBungalow = alojamiento.Tipo == "Bungalow";
                BungalowRadioButton.IsChecked = esBungalow;
                ParcelaRadioButton.IsChecked = !esBungalow;

                CargarCategorias(esBungalow);
                CategoriaComboBox.Text = alojamiento.Categoria;

                // Campos parcela
                AguaCheckBox.IsChecked = alojamiento.TieneAgua;
                LuzCheckBox.IsChecked = alojamiento.TieneLuz;
                DesagueCheckBox.IsChecked = alojamiento.TieneDesague;
                SetosCheckBox.IsChecked = alojamiento.DelimitadaSetos;
                SombraCheckBox.IsChecked = alojamiento.ZonaSombra;
                PrecioBajaTextBox.Text = alojamiento.PrecioTempBaja.ToString("0.##", CultureInfo.CurrentCulture);
                PrecioMediaTextBox.Text = alojamiento.PrecioTempMedia.ToString("0.##", CultureInfo.CurrentCulture);
                PrecioAltaTextBox.Text = alojamiento.PrecioTempAlta.ToString("0.##", CultureInfo.CurrentCulture);

                // Campos bungalow
                PrecioBungalowTextBox.Text = alojamiento.PrecioNoche.ToString("0.##", CultureInfo.CurrentCulture);
                EstanciaMinimaTextBox.Text = alojamiento.EstanciaMinimaNoches.HasValue
                    ? alojamiento.EstanciaMinimaNoches.Value.ToString(CultureInfo.InvariantCulture)
                    : string.Empty;
                MenajeCheckBox.IsChecked = alojamiento.TieneMenaje;
                MicroondasCheckBox.IsChecked = alojamiento.TieneMicroondas;
                RopaCamaCheckBox.IsChecked = alojamiento.TieneRopaCamaToallas;
                WifiCheckBox.IsChecked = alojamiento.TieneWifi;
                TerrazaCheckBox.IsChecked = alojamiento.TieneTerraza;
                AireCheckBox.IsChecked = alojamiento.TieneAireAcondicionado;

                if (alojamiento.Fotos != null)
                {
                    foreach (var f in alojamiento.Fotos)
                    {
                        fotosEditadas.Add(f);
                    }
                }
            }
            else
            {
                ParcelaRadioButton.IsChecked = true;
                CargarCategorias(false);
                CategoriaComboBox.SelectedIndex = 0;
                EstadoComboBox.SelectedIndex = 0;
                UbicacionComboBox.SelectedIndex = 0;
                fotosEditadas.Add("pack://application:,,,/Assets/bungalow_familiar_1.png");
            }

            ActualizarPanelTipo();
        }

        private void CargarCategorias(bool esBungalow)
        {
            if (CategoriaComboBox == null) return;
            CategoriaComboBox.Items.Clear();
            if (esBungalow)
            {
                CategoriaComboBox.Items.Add("Familiar");
                CategoriaComboBox.Items.Add("Pareja");
                CategoriaComboBox.Items.Add("Bosque");
                CategoriaComboBox.Items.Add("Premium");
            }
            else
            {
                CategoriaComboBox.Items.Add("Pequeña");
                CategoriaComboBox.Items.Add("Mediana");
                CategoriaComboBox.Items.Add("Deluxe");
                CategoriaComboBox.Items.Add("Autocaravana");
            }
        }

        private void SeleccionarComboPorTexto(ComboBox combo, string valor)
        {
            if (string.IsNullOrWhiteSpace(valor)) return;
            foreach (ComboBoxItem item in combo.Items)
            {
                if (string.Equals(item.Content as string, valor, StringComparison.OrdinalIgnoreCase))
                {
                    combo.SelectedItem = item;
                    return;
                }
            }
            combo.SelectedIndex = 0;
        }

        private void TipoRadio_Changed(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded) return;
            bool esBungalow = BungalowRadioButton.IsChecked == true;
            CargarCategorias(esBungalow);
            if (CategoriaComboBox.Items.Count > 0) CategoriaComboBox.SelectedIndex = 0;
            ActualizarPanelTipo();
        }

        private void ActualizarPanelTipo()
        {
            if (ParcelaPanel == null || BungalowPanel == null) return;
            bool esBungalow = BungalowRadioButton.IsChecked == true;
            ParcelaPanel.Visibility = esBungalow ? Visibility.Collapsed : Visibility.Visible;
            BungalowPanel.Visibility = esBungalow ? Visibility.Visible : Visibility.Collapsed;
        }

        private void AnadirFoto_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Title = "Seleccionar fotografía del bungalow",
                Filter = "Imágenes|*.png;*.jpg;*.jpeg;*.bmp|Todos los archivos|*.*"
            };
            if (dlg.ShowDialog() == true)
            {
                fotosEditadas.Add(dlg.FileName);
            }
        }

        private void QuitarFoto_Click(object sender, RoutedEventArgs e)
        {
            if (FotosListBox.SelectedItem is string foto)
            {
                fotosEditadas.Remove(foto);
            }
            else
            {
                MostrarError("Seleccione primero la foto que desea quitar.", FotosListBox);
            }
        }

        private void FotosListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ErrorBorder.Visibility = Visibility.Collapsed;
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            string codigo = CodigoTextBox.Text.Trim();
            string categoria = CategoriaComboBox.Text.Trim();
            bool esBungalow = BungalowRadioButton.IsChecked == true;
            string tipo = esBungalow ? "Bungalow" : "Parcela";
            string estado = (EstadoComboBox.SelectedItem as ComboBoxItem)?.Content as string ?? "Libre";

            if (string.IsNullOrWhiteSpace(codigo))
            {
                MostrarError("El código del alojamiento es obligatorio.", CodigoTextBox);
                return;
            }
            bool duplicado = DatosPrueba.Alojamientos.Any(a =>
                a != alojamientoOriginal && a.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase));
            if (duplicado)
            {
                MostrarError("Ya existe otro alojamiento con el código «" + codigo + "».", CodigoTextBox);
                return;
            }
            if (string.IsNullOrWhiteSpace(categoria))
            {
                MostrarError("Indique la categoría del alojamiento.", CategoriaComboBox);
                return;
            }

            if (!TryParseDoubleFlexible(TamanoTextBox.Text, out double tamano) || tamano <= 0)
            {
                MostrarError("El tamaño (m²) debe ser un número positivo.", TamanoTextBox);
                return;
            }
            if (!int.TryParse(CapacidadTextBox.Text.Trim(), out int capacidad) || capacidad <= 0)
            {
                MostrarError("La capacidad máxima debe ser un entero positivo.", CapacidadTextBox);
                return;
            }

            decimal pBaja = 0, pMedia = 0, pAlta = 0, pBungalow = 0;
            int? estanciaMinima = null;

            if (!esBungalow)
            {
                if (!TryParseDecimalFlexible(PrecioBajaTextBox.Text, out pBaja) || pBaja <= 0)
                {
                    MostrarError("La tarifa de temporada baja debe ser mayor que cero.", PrecioBajaTextBox);
                    return;
                }
                if (!TryParseDecimalFlexible(PrecioMediaTextBox.Text, out pMedia) || pMedia <= 0)
                {
                    MostrarError("La tarifa de temporada media debe ser mayor que cero.", PrecioMediaTextBox);
                    return;
                }
                if (!TryParseDecimalFlexible(PrecioAltaTextBox.Text, out pAlta) || pAlta <= 0)
                {
                    MostrarError("La tarifa de temporada alta debe ser mayor que cero.", PrecioAltaTextBox);
                    return;
                }
                pBungalow = pMedia;
            }
            else
            {
                if (!TryParseDecimalFlexible(PrecioBungalowTextBox.Text, out pBungalow) || pBungalow <= 0)
                {
                    MostrarError("El precio por noche del bungalow debe ser mayor que cero.", PrecioBungalowTextBox);
                    return;
                }
                if (!string.IsNullOrWhiteSpace(EstanciaMinimaTextBox.Text))
                {
                    if (!int.TryParse(EstanciaMinimaTextBox.Text.Trim(), out int minNoches) || minNoches <= 0)
                    {
                        MostrarError("La estancia mínima debe ser un entero positivo o quedar vacía.", EstanciaMinimaTextBox);
                        return;
                    }
                    estanciaMinima = minNoches;
                }
                pBaja = pMedia = pAlta = pBungalow;
            }

            Alojamiento destino = alojamientoOriginal ?? new Alojamiento();
            destino.Codigo = codigo;
            destino.Tipo = tipo;
            destino.Categoria = categoria;
            destino.TamanoM2 = tamano;
            destino.Ubicacion = UbicacionComboBox.Text.Trim();
            destino.Capacidad = capacidad;
            destino.Estado = estado;
            destino.Descripcion = DescripcionTextBox.Text.Trim();

            destino.PrecioNoche = pBungalow;
            destino.PrecioTempBaja = pBaja;
            destino.PrecioTempMedia = pMedia;
            destino.PrecioTempAlta = pAlta;
            destino.EstanciaMinimaNoches = estanciaMinima;

            destino.TieneAgua = !esBungalow && AguaCheckBox.IsChecked == true;
            destino.TieneLuz = !esBungalow && LuzCheckBox.IsChecked == true;
            destino.TieneDesague = !esBungalow && DesagueCheckBox.IsChecked == true;
            destino.DelimitadaSetos = !esBungalow && SetosCheckBox.IsChecked == true;
            destino.ZonaSombra = !esBungalow && SombraCheckBox.IsChecked == true;

            destino.TieneMenaje = esBungalow && MenajeCheckBox.IsChecked == true;
            destino.TieneMicroondas = esBungalow && MicroondasCheckBox.IsChecked == true;
            destino.TieneRopaCamaToallas = esBungalow && RopaCamaCheckBox.IsChecked == true;
            destino.TieneWifi = esBungalow && WifiCheckBox.IsChecked == true;
            destino.TieneTerraza = esBungalow && TerrazaCheckBox.IsChecked == true;
            destino.TieneAireAcondicionado = esBungalow && AireCheckBox.IsChecked == true;

            destino.Fotos.Clear();
            if (esBungalow)
            {
                foreach (var f in fotosEditadas) destino.Fotos.Add(f);
            }
            else
            {
                string defaultImg = categoria.ToLowerInvariant().Contains("autocaravana")
                    ? "pack://application:,,,/Assets/parcela_autocaravana.png"
                    : "pack://application:,,,/Assets/parcela_piscina.png";
                destino.Fotos.Add(defaultImg);
            }

            destino.RefrescarPropiedadesCalculadas();
            AlojamientoGuardado = destino;
            DialogResult = true;
        }

        private static bool TryParseDecimalFlexible(string raw, out decimal val)
        {
            string norm = (raw ?? string.Empty).Trim().Replace(',', '.');
            return decimal.TryParse(norm, NumberStyles.Number, CultureInfo.InvariantCulture, out val);
        }

        private static bool TryParseDoubleFlexible(string raw, out double val)
        {
            string norm = (raw ?? string.Empty).Trim().Replace(',', '.');
            return double.TryParse(norm, NumberStyles.Float, CultureInfo.InvariantCulture, out val);
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
            new AyudaWindow("Alojamientos") { Owner = this }.ShowDialog();
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
