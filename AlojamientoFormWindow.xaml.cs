using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Trabajo_IPO
{
    public partial class AlojamientoFormWindow : Window
    {
        private readonly Alojamiento alojamientoOriginal;

        public Alojamiento AlojamientoGuardado { get; private set; }

        public AlojamientoFormWindow(Alojamiento alojamiento = null)
        {
            InitializeComponent();
            alojamientoOriginal = alojamiento;
            TituloTextBlock.Text = alojamiento == null ? "Nuevo alojamiento" : "Modificar alojamiento";

            if (alojamiento != null)
            {
                CodigoTextBox.Text = alojamiento.Codigo;
                CapacidadTextBox.Text = alojamiento.Capacidad.ToString(CultureInfo.InvariantCulture);
                PrecioTextBox.Text = alojamiento.PrecioNoche.ToString(CultureInfo.InvariantCulture);
                UbicacionTextBox.Text = alojamiento.Ubicacion;
                TipoComboBox.SelectedIndex = alojamiento.Tipo == "Bungalow" ? 1 : 0;
                SetosCheckBox.IsChecked = alojamiento.Caracteristicas != null &&
                    alojamiento.Caracteristicas.ToLowerInvariant().Contains("seto");
                WifiCheckBox.IsChecked = alojamiento.Caracteristicas != null &&
                    alojamiento.Caracteristicas.ToLowerInvariant().Contains("wi-fi");
            }
            else
            {
                TipoComboBox.SelectedIndex = 0;
            }
        }

        private void TipoComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool esParcela = TipoComboBox.SelectedIndex == 0;
            SetosCheckBox.Visibility = esParcela ? Visibility.Visible : Visibility.Collapsed;
            WifiCheckBox.Visibility = esParcela ? Visibility.Collapsed : Visibility.Visible;
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            int capacidad;
            decimal precio;
            string codigo = CodigoTextBox.Text.Trim();
            string tipo = TipoComboBox.SelectedIndex == 1 ? "Bungalow" : "Parcela";

            if (string.IsNullOrWhiteSpace(codigo))
            {
                MostrarError("El código es obligatorio.", CodigoTextBox);
                return;
            }
            if (!int.TryParse(CapacidadTextBox.Text.Trim(), out capacidad) || capacidad <= 0)
            {
                MostrarError("La capacidad debe ser un número entero mayor que cero.", CapacidadTextBox);
                return;
            }
            if (!decimal.TryParse(PrecioTextBox.Text.Trim(), NumberStyles.Number,
                CultureInfo.CurrentCulture, out precio) || precio < 0)
            {
                MostrarError("El precio debe ser un número válido igual o superior a cero.", PrecioTextBox);
                return;
            }

            bool codigoDuplicado = DatosPrueba.Alojamientos.Any(a =>
                a != alojamientoOriginal && a.Codigo.Equals(codigo, System.StringComparison.OrdinalIgnoreCase));
            if (codigoDuplicado)
            {
                MostrarError("Ya existe un alojamiento con ese código.", CodigoTextBox);
                return;
            }

            string caracteristicas;
            if (tipo == "Parcela")
            {
                caracteristicas = SetosCheckBox.IsChecked == true
                    ? "Delimitada por setos" : "Parcela sin delimitar";
            }
            else
            {
                caracteristicas = WifiCheckBox.IsChecked == true
                    ? "Dispone de Wi-Fi" : "Sin Wi-Fi";
            }

            if (alojamientoOriginal == null)
            {
                AlojamientoGuardado = new Alojamiento
                {
                    Codigo = codigo,
                    Tipo = tipo,
                    Categoria = tipo,
                    Ubicacion = UbicacionTextBox.Text.Trim(),
                    Capacidad = capacidad,
                    PrecioNoche = precio,
                    Estado = "Libre",
                    Caracteristicas = caracteristicas
                };
            }
            else
            {
                alojamientoOriginal.Codigo = codigo;
                alojamientoOriginal.Tipo = tipo;
                alojamientoOriginal.Ubicacion = UbicacionTextBox.Text.Trim();
                alojamientoOriginal.Capacidad = capacidad;
                alojamientoOriginal.PrecioNoche = precio;
                alojamientoOriginal.Caracteristicas = caracteristicas;
                AlojamientoGuardado = alojamientoOriginal;
            }

            DialogResult = true;
        }

        private void MostrarError(string mensaje, Control control)
        {
            MessageBox.Show(mensaje, "Revisar datos", MessageBoxButton.OK, MessageBoxImage.Warning);
            control.Focus();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
