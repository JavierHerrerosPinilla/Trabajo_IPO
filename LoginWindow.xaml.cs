using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Trabajo_IPO
{
    public partial class LoginWindow : Window
    {
        private bool sincronizandoPassword;

        public LoginWindow()
        {
            InitializeComponent();
            LogoImage.Source = new ImagePathConverter().Convert(
                "pack://application:,,,/Assets/logo_camping.png",
                typeof(ImageSource), null, System.Globalization.CultureInfo.CurrentCulture) as ImageSource;
            UsuarioTextBox.Focus();
        }

        private string ObtenerPasswordActual()
        {
            return MostrarContrasenaCheckBox.IsChecked == true
                ? ContrasenaVisibleTextBox.Text
                : ContrasenaBox.Password;
        }

        private void IniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            string usuario = UsuarioTextBox.Text.Trim();
            string password = ObtenerPasswordActual();

            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password))
            {
                MostrarError("Introduzca su usuario y contraseña para continuar.");
                if (string.IsNullOrWhiteSpace(usuario))
                    UsuarioTextBox.Focus();
                else if (MostrarContrasenaCheckBox.IsChecked == true)
                    ContrasenaVisibleTextBox.Focus();
                else
                    ContrasenaBox.Focus();
                return;
            }

            Gestor gestor = DatosPrueba.Gestores.FirstOrDefault(g =>
                g.Usuario.Equals(usuario, StringComparison.OrdinalIgnoreCase) &&
                g.Contrasena == password);

            if (gestor == null)
            {
                MostrarError("Usuario o contraseña incorrectos. Revise sus credenciales e inténtelo de nuevo.");
                ContrasenaBox.Clear();
                ContrasenaVisibleTextBox.Clear();
                if (MostrarContrasenaCheckBox.IsChecked == true)
                    ContrasenaVisibleTextBox.Focus();
                else
                    ContrasenaBox.Focus();
                return;
            }

            DateTime accesoPrevio = gestor.UltimoAcceso;
            MainWindow ventanaPrincipal = new MainWindow(gestor, accesoPrevio);
            gestor.UltimoAcceso = DateTime.Now;
            Application.Current.MainWindow = ventanaPrincipal;
            ventanaPrincipal.Show();
            Close();
        }

        private void MostrarError(string mensaje)
        {
            ErrorTextBlock.Text = mensaje;
            ErrorBorder.Visibility = Visibility.Visible;
            var danger = (Brush)FindResource("DangerBrush");
            UsuarioTextBox.BorderBrush = danger;
            ContrasenaBox.BorderBrush = danger;
            ContrasenaVisibleTextBox.BorderBrush = danger;
        }

        private void LimpiarErrorVisual()
        {
            ErrorBorder.Visibility = Visibility.Collapsed;
            var normal = new SolidColorBrush(Color.FromRgb(184, 196, 208));
            UsuarioTextBox.BorderBrush = normal;
            ContrasenaBox.BorderBrush = normal;
            ContrasenaVisibleTextBox.BorderBrush = normal;
        }

        private void Campos_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            LimpiarErrorVisual();
        }

        private void ContrasenaBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sincronizandoPassword) return;
            sincronizandoPassword = true;
            ContrasenaVisibleTextBox.Text = ContrasenaBox.Password;
            sincronizandoPassword = false;
            LimpiarErrorVisual();
        }

        private void ContrasenaVisibleTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (sincronizandoPassword) return;
            sincronizandoPassword = true;
            ContrasenaBox.Password = ContrasenaVisibleTextBox.Text;
            sincronizandoPassword = false;
            LimpiarErrorVisual();
        }

        private void MostrarContrasenaCheckBox_Changed(object sender, RoutedEventArgs e)
        {
            bool visible = MostrarContrasenaCheckBox.IsChecked == true;
            ContrasenaVisibleTextBox.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            ContrasenaBox.Visibility = visible ? Visibility.Collapsed : Visibility.Visible;
            if (visible)
                ContrasenaVisibleTextBox.Focus();
            else
                ContrasenaBox.Focus();
        }

        private void Limpiar_Click(object sender, RoutedEventArgs e)
        {
            UsuarioTextBox.Clear();
            ContrasenaBox.Clear();
            ContrasenaVisibleTextBox.Clear();
            LimpiarErrorVisual();
            UsuarioTextBox.Focus();
        }

        private void Ayuda_Click(object sender, RoutedEventArgs e)
        {
            var ayuda = new AyudaWindow { Owner = this };
            ayuda.ShowDialog();
        }

        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            var r = MessageBox.Show(
                "¿Desea salir de la aplicación?",
                "Confirmar salida",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No);

            if (r == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
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
