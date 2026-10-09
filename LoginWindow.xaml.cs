using System.Linq;
using System.Windows;

namespace Trabajo_IPO
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            UsuarioTextBox.Focus();
        }

        private void IniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            Gestor gestor = DatosPrueba.Gestores.FirstOrDefault(g =>
                g.Usuario == UsuarioTextBox.Text.Trim() &&
                g.Contrasena == ContrasenaBox.Password);

            if (gestor == null)
            {
                MessageBox.Show("El usuario o la contraseña no son correctos.", "Acceso no válido",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                ContrasenaBox.Clear();
                ContrasenaBox.Focus();
                return;
            }

            MainWindow ventanaPrincipal = new MainWindow(gestor);
            Application.Current.MainWindow = ventanaPrincipal;
            ventanaPrincipal.Show();
            Close();
        }

        private void Limpiar_Click(object sender, RoutedEventArgs e)
        {
            UsuarioTextBox.Clear();
            ContrasenaBox.Clear();
            UsuarioTextBox.Focus();
        }
    }
}
