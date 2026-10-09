using System.Windows;
using System.Windows.Controls;

namespace Trabajo_IPO
{
    public partial class MainWindow : Window
    {
        public MainWindow(Gestor gestor)
        {
            InitializeComponent();
            DataContext = new
            {
                gestor.NombreCompleto,
                gestor.Rol,
                gestor.FotoPerfil,
                gestor.UltimoAcceso,
                Alojamientos = DatosPrueba.Alojamientos,
                Reservas = DatosPrueba.Reservas,
                Empleados = DatosPrueba.Empleados,
                Actividades = DatosPrueba.Actividades
            };
        }

        private void AnadirAlojamiento_Click(object sender, RoutedEventArgs e)
        {
            AlojamientoFormWindow formulario = new AlojamientoFormWindow
            {
                Owner = this
            };

            if (formulario.ShowDialog() == true)
            {
                DatosPrueba.Alojamientos.Add(formulario.AlojamientoGuardado);
                MessageBox.Show("El alojamiento se ha creado correctamente.", "Operación completada",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ModificarAlojamiento_Click(object sender, RoutedEventArgs e)
        {
            Alojamiento alojamiento = AlojamientosGrid.SelectedItem as Alojamiento;
            if (alojamiento == null)
            {
                MessageBox.Show("Seleccione primero un alojamiento.", "Selección necesaria",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            AlojamientoFormWindow formulario = new AlojamientoFormWindow(alojamiento)
            {
                Owner = this
            };

            if (formulario.ShowDialog() == true)
            {
                AlojamientosGrid.Items.Refresh();
                MessageBox.Show("El alojamiento se ha actualizado correctamente.", "Operación completada",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Ayuda_Click(object sender, RoutedEventArgs e)
        {
            AyudaWindow ayuda = new AyudaWindow
            {
                Owner = this
            };
            ayuda.ShowDialog();
        }

        private void AnadirReserva_Click(object sender, RoutedEventArgs e)
        {
            ReservaFormWindow formulario = new ReservaFormWindow
            {
                Owner = this
            };

            if (formulario.ShowDialog() == true)
            {
                DatosPrueba.Reservas.Add(formulario.ReservaGuardada);
                MessageBox.Show("La reserva se ha creado correctamente.", "Operación completada",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ModificarReserva_Click(object sender, RoutedEventArgs e)
        {
            Reserva reserva = ReservasGrid.SelectedItem as Reserva;
            if (reserva == null)
            {
                MessageBox.Show("Seleccione primero una reserva.", "Selección necesaria",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            ReservaFormWindow formulario = new ReservaFormWindow(reserva)
            {
                Owner = this
            };

            if (formulario.ShowDialog() == true)
            {
                ReservasGrid.Items.Refresh();
                MessageBox.Show("La reserva se ha actualizado correctamente.", "Operación completada",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void FuenteGrandeToggleButton_Click(object sender, RoutedEventArgs e)
        {
            FontSize = FuenteGrandeToggleButton.IsChecked == true ? 16 : 14;
            FuenteGrandeToggleButton.Content = FuenteGrandeToggleButton.IsChecked == true ? "A-" : "A+";
        }

        private void EliminarAlojamiento_Click(object sender, RoutedEventArgs e)
        {
            Alojamiento alojamiento = AlojamientosGrid.SelectedItem as Alojamiento;
            if (alojamiento == null)
            {
                MessageBox.Show("Seleccione primero un alojamiento.", "Selección necesaria",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (ConfirmarEliminacion(alojamiento.Codigo))
            {
                DatosPrueba.Alojamientos.Remove(alojamiento);
                MessageBox.Show("El alojamiento se ha eliminado correctamente.", "Operación completada",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void EliminarReserva_Click(object sender, RoutedEventArgs e)
        {
            Reserva reserva = ReservasGrid.SelectedItem as Reserva;
            if (reserva == null)
            {
                MessageBox.Show("Seleccione primero una reserva.", "Selección necesaria",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (ConfirmarEliminacion(reserva.Codigo))
            {
                DatosPrueba.Reservas.Remove(reserva);
                MessageBox.Show("La reserva se ha cancelado correctamente.", "Operación completada",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private bool ConfirmarEliminacion(string identificador)
        {
            MessageBoxResult respuesta = MessageBox.Show(
                "¿Está seguro de que desea eliminar este registro?\n\nRegistro: " + identificador,
                "Confirmar eliminación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.No);

            return respuesta == MessageBoxResult.Yes;
        }
    }
}
