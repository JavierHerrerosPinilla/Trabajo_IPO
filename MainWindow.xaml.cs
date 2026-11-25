using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Trabajo_IPO
{
    public partial class MainWindow : Window
    {
        private readonly Gestor gestorActual;
        private bool cerrandoSesion;
        private DateTime fechaInicioCuadrante = new DateTime(2026, 7, 1);

        public MainWindow(Gestor gestor, DateTime? ultimoAccesoPrevio = null)
        {
            InitializeComponent();
            gestorActual = gestor;
            HeaderLogoImage.Source = new ImagePathConverter().Convert(
                "pack://application:,,,/Assets/logo_camping.png",
                typeof(ImageSource), null, CultureInfo.CurrentCulture) as ImageSource;
            DataContext = new
            {
                gestor.NombreCompleto,
                gestor.Rol,
                gestor.FotoPerfil,
                UltimoAccesoPrevio = ultimoAccesoPrevio ?? gestor.UltimoAcceso
            };

            PoblarCombosDinamicos();
            AplicarFiltrosAlojamientos();
            AplicarFiltrosReservas();
            AplicarFiltrosEmpleados();
            AplicarFiltrosActividades();
            ConstruirCuadranteDisponibilidad();

            if (AlojamientosGrid.Items.Count > 0) AlojamientosGrid.SelectedIndex = 0;
            if (ReservasGrid.Items.Count > 0) ReservasGrid.SelectedIndex = 0;
            if (EmpleadosGrid.Items.Count > 0) EmpleadosGrid.SelectedIndex = 0;
            if (ActividadesGrid.Items.Count > 0) ActividadesGrid.SelectedIndex = 0;
        }

        private void PoblarCombosDinamicos()
        {
            FiltroResAlojCombo.Items.Clear();
            FiltroResAlojCombo.Items.Add("Todos");
            foreach (var a in DatosPrueba.Alojamientos.OrderBy(x => x.Codigo))
            {
                FiltroResAlojCombo.Items.Add(a.Codigo);
            }
            FiltroResAlojCombo.SelectedIndex = 0;

            FiltroActZonaCombo.Items.Clear();
            FiltroActZonaCombo.Items.Add("Todas");
            foreach (var z in DatosPrueba.Actividades.Select(x => x.Zona).Where(z => !string.IsNullOrWhiteSpace(z)).Distinct().OrderBy(z => z))
            {
                FiltroActZonaCombo.Items.Add(z);
            }
            FiltroActZonaCombo.SelectedIndex = 0;
        }

        private void InformarEstado(string mensaje, bool esError = false)
        {
            string prefijo = esError ? "⚠ " : "✓ ";
            StatusTextBlock.Text = $"{prefijo}{mensaje} ({DateTime.Now:HH:mm:ss})";
            StatusTextBlock.Foreground = (Brush)FindResource(esError ? "DangerBrush" : "SuccessBrush");
        }

        // ==================== ADAPTACIÓN Y CONTROLES GLOBALES ====================

        private void FuenteDisminuir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources["AppFontSize"] = 12.0;
            InformarEstado("Tamaño de fuente reducido (12 pt)");
        }

        private void FuenteNormal_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources["AppFontSize"] = 13.5;
            InformarEstado("Tamaño de fuente estándar (13.5 pt)");
        }

        private void FuenteAumentar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources["AppFontSize"] = 16.0;
            InformarEstado("Tamaño de fuente grande activado (16 pt)");
        }

        private void ContrasteToggleButton_Click(object sender, RoutedEventArgs e)
        {
            bool alto = ContrasteToggleButton.IsChecked == true;
            if (alto)
            {
                Application.Current.Resources["AppBgBrush"] = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                Application.Current.Resources["CardBgBrush"] = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                Application.Current.Resources["HeaderBgBrush"] = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                Application.Current.Resources["PrimaryBrush"] = new SolidColorBrush(Color.FromRgb(0, 51, 153));
                Application.Current.Resources["TextMainBrush"] = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                Application.Current.Resources["BorderBrushSoft"] = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                InformarEstado("Modo de alto contraste activado");
            }
            else
            {
                Application.Current.Resources["AppBgBrush"] = new SolidColorBrush(Color.FromRgb(245, 247, 250));
                Application.Current.Resources["CardBgBrush"] = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                Application.Current.Resources["HeaderBgBrush"] = new SolidColorBrush(Color.FromRgb(23, 50, 77));
                Application.Current.Resources["PrimaryBrush"] = new SolidColorBrush(Color.FromRgb(30, 96, 145));
                Application.Current.Resources["TextMainBrush"] = new SolidColorBrush(Color.FromRgb(31, 41, 51));
                Application.Current.Resources["BorderBrushSoft"] = new SolidColorBrush(Color.FromRgb(209, 216, 224));
                InformarEstado("Paleta estándar restaurada");
            }
        }

        private void Ayuda_Click(object sender, RoutedEventArgs e)
        {
            string[] temas = { "Alojamientos", "Disponibilidad", "Reservas", "Empleados", "Actividades" };
            int idx = PrincipalTabControl.SelectedIndex;
            string tema = (idx >= 0 && idx < temas.Length) ? temas[idx] : "Primeros pasos";
            new AyudaWindow(tema) { Owner = this }.ShowDialog();
        }

        private void CerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            var r = MessageBox.Show(
                "¿Desea cerrar la sesión actual y volver a la pantalla de acceso?",
                "Cerrar sesión",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No);

            if (r == MessageBoxResult.Yes)
            {
                cerrandoSesion = true;
                var login = new LoginWindow();
                Application.Current.MainWindow = login;
                login.Show();
                Close();
            }
        }

        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (cerrandoSesion) return;
            var r = MessageBox.Show(
                "¿Está seguro de que desea salir de la aplicación?\n\nLos datos de prueba no guardados fuera de esta sesión se perderán.",
                "Confirmar salida",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No);

            if (r != MessageBoxResult.Yes)
            {
                e.Cancel = true;
            }
        }

        // ==================== ATAJOS DE TECLADO ====================

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                Ayuda_Click(sender, e);
                e.Handled = true;
                return;
            }

            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                switch (e.Key)
                {
                    case Key.D1: case Key.NumPad1: PrincipalTabControl.SelectedIndex = 0; e.Handled = true; break;
                    case Key.D2: case Key.NumPad2: PrincipalTabControl.SelectedIndex = 1; e.Handled = true; break;
                    case Key.D3: case Key.NumPad3: PrincipalTabControl.SelectedIndex = 2; e.Handled = true; break;
                    case Key.D4: case Key.NumPad4: PrincipalTabControl.SelectedIndex = 3; e.Handled = true; break;
                    case Key.D5: case Key.NumPad5: PrincipalTabControl.SelectedIndex = 4; e.Handled = true; break;
                    case Key.N:
                        EjecutarNuevoPorPestana();
                        e.Handled = true;
                        break;
                    case Key.E:
                        EjecutarEditarPorPestana();
                        e.Handled = true;
                        break;
                }
            }
            else if (e.Key == Key.Delete && !(Keyboard.FocusedElement is TextBox))
            {
                EjecutarEliminarPorPestana();
                e.Handled = true;
            }
        }

        private void EjecutarNuevoPorPestana()
        {
            switch (PrincipalTabControl.SelectedIndex)
            {
                case 0: AnadirAlojamiento_Click(this, new RoutedEventArgs()); break;
                case 1:
                case 2: AnadirReserva_Click(this, new RoutedEventArgs()); break;
                case 3: AnadirEmpleado_Click(this, new RoutedEventArgs()); break;
                case 4: AnadirActividad_Click(this, new RoutedEventArgs()); break;
            }
        }

        private void EjecutarEditarPorPestana()
        {
            switch (PrincipalTabControl.SelectedIndex)
            {
                case 0: ModificarAlojamiento_Click(this, new RoutedEventArgs()); break;
                case 2: ModificarReserva_Click(this, new RoutedEventArgs()); break;
                case 3: ModificarEmpleado_Click(this, new RoutedEventArgs()); break;
                case 4: ModificarActividad_Click(this, new RoutedEventArgs()); break;
            }
        }

        private void EjecutarEliminarPorPestana()
        {
            switch (PrincipalTabControl.SelectedIndex)
            {
                case 0: EliminarAlojamiento_Click(this, new RoutedEventArgs()); break;
                case 2: EliminarReserva_Click(this, new RoutedEventArgs()); break;
                case 3: EliminarEmpleado_Click(this, new RoutedEventArgs()); break;
                case 4: EliminarActividad_Click(this, new RoutedEventArgs()); break;
            }
        }

        // ==================== PESTAÑA 1: ALOJAMIENTOS ====================

        private void AplicarFiltrosAlojamientos()
        {
            if (AlojamientosGrid == null) return;
            string tipo = (FiltroAlojTipoCombo.SelectedItem as ComboBoxItem)?.Content as string ?? "Todos";
            string estado = (FiltroAlojEstadoCombo.SelectedItem as ComboBoxItem)?.Content as string ?? "Todos";
            string capTxt = (FiltroAlojCapCombo.SelectedItem as ComboBoxItem)?.Content as string ?? "Todas";
            string busq = (FiltroAlojBuscarTextBox.Text ?? string.Empty).Trim().ToLowerInvariant();
            int capMin = int.TryParse(capTxt, out int c) ? c : 0;

            var filtrados = DatosPrueba.Alojamientos.Where(a =>
                (tipo == "Todos" || a.Tipo == tipo) &&
                (estado == "Todos" || a.Estado == estado) &&
                a.Capacidad >= capMin &&
                (string.IsNullOrEmpty(busq) ||
                 (a.Codigo ?? "").ToLowerInvariant().Contains(busq) ||
                 (a.Categoria ?? "").ToLowerInvariant().Contains(busq) ||
                 (a.Ubicacion ?? "").ToLowerInvariant().Contains(busq))).ToList();

            var sel = AlojamientosGrid.SelectedItem as Alojamiento;
            AlojamientosGrid.ItemsSource = filtrados;
            if (sel != null && filtrados.Contains(sel)) AlojamientosGrid.SelectedItem = sel;
            else if (filtrados.Count > 0) AlojamientosGrid.SelectedIndex = 0;

            ContadorAlojTextBlock.Text = $"{filtrados.Count} de {DatosPrueba.Alojamientos.Count} alojamientos";
            ActualizarDetalleAlojamiento();
        }

        private void FiltroAlojamientos_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded) AplicarFiltrosAlojamientos();
        }

        private void FiltroAlojamientosText_Changed(object sender, TextChangedEventArgs e)
        {
            if (IsLoaded) AplicarFiltrosAlojamientos();
        }

        private void LimpiarFiltrosAlojamientos_Click(object sender, RoutedEventArgs e)
        {
            FiltroAlojTipoCombo.SelectedIndex = 0;
            FiltroAlojEstadoCombo.SelectedIndex = 0;
            FiltroAlojCapCombo.SelectedIndex = 0;
            FiltroAlojBuscarTextBox.Clear();
            AplicarFiltrosAlojamientos();
        }

        private void AlojamientosGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ActualizarDetalleAlojamiento();
        }

        private void AlojamientosGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (AlojamientosGrid.SelectedItem is Alojamiento)
                ModificarAlojamiento_Click(sender, e);
        }

        private void ActualizarDetalleAlojamiento()
        {
            if (DetalleAlojTitulo == null) return;
            var a = AlojamientosGrid.SelectedItem as Alojamiento;
            if (a == null)
            {
                DetalleAlojTitulo.Text = "Seleccione un alojamiento para consultar su ficha";
                DetalleAlojTarifas.Text = string.Empty;
                DetalleAlojServicios.Text = string.Empty;
                DetalleAlojDescripcion.Text = string.Empty;
                DetalleAlojGaleriaListBox.ItemsSource = null;
                DetalleAlojFotoPrincipal.Source = null;
                return;
            }

            DetalleAlojTitulo.Text = $"{a.Codigo} · {a.Tipo} {a.Categoria} · {a.TamanoTexto} · Capacidad: {a.Capacidad} pers. · Ubicación: {a.Ubicacion}";
            DetalleAlojTarifas.Text = a.TarifasDetalle;
            DetalleAlojServicios.Text = (a.Tipo == "Parcela" ? "Servicios: " : "Equipamiento: ") + a.Caracteristicas;
            DetalleAlojDescripcion.Text = string.IsNullOrWhiteSpace(a.Descripcion) ? "Sin descripción adicional." : a.Descripcion;

            DetalleAlojGaleriaListBox.ItemsSource = a.Fotos;
            if (a.Fotos != null && a.Fotos.Count > 0)
            {
                DetalleAlojGaleriaListBox.SelectedIndex = 0;
                var conv = new ImagePathConverter();
                DetalleAlojFotoPrincipal.Source = conv.Convert(a.Fotos[0], typeof(ImageSource), null, CultureInfo.CurrentCulture) as ImageSource;
            }
            else
            {
                DetalleAlojFotoPrincipal.Source = null;
            }
        }

        private void DetalleAlojGaleriaListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DetalleAlojGaleriaListBox.SelectedItem is string ruta)
            {
                var conv = new ImagePathConverter();
                DetalleAlojFotoPrincipal.Source = conv.Convert(ruta, typeof(ImageSource), null, CultureInfo.CurrentCulture) as ImageSource;
            }
        }

        private void AnadirAlojamiento_Click(object sender, RoutedEventArgs e)
        {
            var form = new AlojamientoFormWindow { Owner = this };
            if (form.ShowDialog() == true)
            {
                DatosPrueba.Alojamientos.Add(form.AlojamientoGuardado);
                PoblarCombosDinamicos();
                AplicarFiltrosAlojamientos();
                AlojamientosGrid.SelectedItem = form.AlojamientoGuardado;
                ConstruirCuadranteDisponibilidad();
                InformarEstado($"Alojamiento {form.AlojamientoGuardado.Codigo} añadido correctamente.");
            }
        }

        private void ModificarAlojamiento_Click(object sender, RoutedEventArgs e)
        {
            if (!(AlojamientosGrid.SelectedItem is Alojamiento a))
            {
                MessageBox.Show("Seleccione primero un alojamiento.", "Selección necesaria",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var form = new AlojamientoFormWindow(a) { Owner = this };
            if (form.ShowDialog() == true)
            {
                PoblarCombosDinamicos();
                AplicarFiltrosAlojamientos();
                ConstruirCuadranteDisponibilidad();
                InformarEstado($"Alojamiento {a.Codigo} actualizado correctamente.");
            }
        }

        private void CambiarEstadoAlojamiento_Click(object sender, RoutedEventArgs e)
        {
            if (!(AlojamientosGrid.SelectedItem is Alojamiento a))
            {
                MessageBox.Show("Seleccione primero un alojamiento.", "Selección necesaria",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            string[] estados = { "Libre", "Ocupada", "Reservada", "En reparación", "Pendiente de limpieza" };
            int pos = Array.IndexOf(estados, a.Estado);
            string siguiente = estados[(pos + 1) % estados.Length];

            var resp = MessageBox.Show(
                $"¿Cambiar el estado del alojamiento {a.Codigo} de «{a.Estado}» a «{siguiente}»?",
                "Cambiar estado de alojamiento",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No);

            if (resp == MessageBoxResult.Yes)
            {
                a.Estado = siguiente;
                AplicarFiltrosAlojamientos();
                ConstruirCuadranteDisponibilidad();
                InformarEstado($"Estado de {a.Codigo} actualizado a «{siguiente}».");
            }
        }

        private void EliminarAlojamiento_Click(object sender, RoutedEventArgs e)
        {
            if (!(AlojamientosGrid.SelectedItem is Alojamiento a))
            {
                MessageBox.Show("Seleccione primero un alojamiento.", "Selección necesaria",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            int reservasActivas = DatosPrueba.Reservas.Count(r =>
                r.Alojamiento == a.Codigo &&
                r.Estado != "Cancelada" &&
                r.Estado != "Finalizada");

            string avisoReservas = reservasActivas > 0
                ? $"\n\n⚠ Atención: este alojamiento tiene {reservasActivas} reserva(s) activa(s) o futura(s)."
                : string.Empty;

            if (ConfirmarEliminacion($"Alojamiento {a.Codigo} ({a.Tipo} {a.Categoria}){avisoReservas}"))
            {
                DatosPrueba.Alojamientos.Remove(a);
                PoblarCombosDinamicos();
                AplicarFiltrosAlojamientos();
                ConstruirCuadranteDisponibilidad();
                InformarEstado($"Alojamiento {a.Codigo} eliminado.");
            }
        }

        // ==================== PESTAÑA 2: DISPONIBILIDAD ====================

        private void DispAnterior_Click(object sender, RoutedEventArgs e)
        {
            fechaInicioCuadrante = fechaInicioCuadrante.AddDays(-15);
            ConstruirCuadranteDisponibilidad();
        }

        private void DispSiguiente_Click(object sender, RoutedEventArgs e)
        {
            fechaInicioCuadrante = fechaInicioCuadrante.AddDays(15);
            ConstruirCuadranteDisponibilidad();
        }

        private void DispRefrescar_Click(object sender, RoutedEventArgs e)
        {
            ConstruirCuadranteDisponibilidad();
            InformarEstado("Cuadrante de disponibilidad actualizado.");
        }

        private void FiltroDisp_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded) ConstruirCuadranteDisponibilidad();
        }

        private void ConstruirCuadranteDisponibilidad()
        {
            if (DisponibilidadCuadranteGrid == null) return;

            DateTime fin = fechaInicioCuadrante.AddDays(14);
            DispPeriodoTextBlock.Text = $"{fechaInicioCuadrante:dd MMM} – {fin:dd MMM yyyy}";

            DisponibilidadCuadranteGrid.Children.Clear();
            DisponibilidadCuadranteGrid.RowDefinitions.Clear();
            DisponibilidadCuadranteGrid.ColumnDefinitions.Clear();

            string tipo = (FiltroDispTipoCombo.SelectedItem as ComboBoxItem)?.Content as string ?? "Todos";
            string estadoFiltro = (FiltroDispEstadoCombo.SelectedItem as ComboBoxItem)?.Content as string ?? "Todos";

            var alojamientos = DatosPrueba.Alojamientos.Where(a =>
                (tipo == "Todos" || a.Tipo == tipo) &&
                (estadoFiltro == "Todos" || a.Estado == estadoFiltro)).ToList();

            const int dias = 15;
            DisponibilidadCuadranteGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(165) });
            for (int d = 0; d < dias; d++)
            {
                DisponibilidadCuadranteGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(68) });
            }

            // Cabecera días
            DisponibilidadCuadranteGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(42) });
            var hdrAloj = CrearCeldaCabecera("Alojamiento", HorizontalAlignment.Left);
            Grid.SetRow(hdrAloj, 0);
            Grid.SetColumn(hdrAloj, 0);
            DisponibilidadCuadranteGrid.Children.Add(hdrAloj);

            for (int d = 0; d < dias; d++)
            {
                DateTime dia = fechaInicioCuadrante.AddDays(d);
                var hdrDia = CrearCeldaCabecera($"{dia:dd}\n{dia:ddd}", HorizontalAlignment.Center);
                Grid.SetRow(hdrDia, 0);
                Grid.SetColumn(hdrDia, d + 1);
                DisponibilidadCuadranteGrid.Children.Add(hdrDia);
            }

            var bgConv = (EstadoToBackgroundConverter)FindResource("EstadoBgConverter");
            var fgConv = (EstadoToForegroundConverter)FindResource("EstadoFgConverter");
            var icConv = (EstadoToIconConverter)FindResource("EstadoIconConverter");

            for (int r = 0; r < alojamientos.Count; r++)
            {
                var aloj = alojamientos[r];
                DisponibilidadCuadranteGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(46) });

                var lblBorder = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(248, 250, 252)),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(220, 226, 234)),
                    BorderThickness = new Thickness(0, 0, 1, 1),
                    Padding = new Thickness(8, 4, 8, 4),
                    Child = new TextBlock
                    {
                        Text = $"{aloj.Codigo} ({aloj.Tipo})",
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush(Color.FromRgb(23, 50, 77)),
                        VerticalAlignment = VerticalAlignment.Center
                    }
                };
                Grid.SetRow(lblBorder, r + 1);
                Grid.SetColumn(lblBorder, 0);
                DisponibilidadCuadranteGrid.Children.Add(lblBorder);

                for (int d = 0; d < dias; d++)
                {
                    DateTime dia = fechaInicioCuadrante.AddDays(d);
                    string est = DatosPrueba.ObtenerEstadoAlojamientoEnFecha(aloj, dia);
                    string abrev = AbreviarEstado(est);
                    string ico = icConv.Convert(est, typeof(string), null, CultureInfo.CurrentCulture) as string;

                    var btn = new Button
                    {
                        Margin = new Thickness(2),
                        Padding = new Thickness(2),
                        Cursor = Cursors.Hand,
                        Background = bgConv.Convert(est, typeof(Brush), null, CultureInfo.CurrentCulture) as Brush,
                        Foreground = fgConv.Convert(est, typeof(Brush), null, CultureInfo.CurrentCulture) as Brush,
                        BorderBrush = new SolidColorBrush(Color.FromRgb(209, 216, 224)),
                        BorderThickness = new Thickness(1),
                        Content = $"{ico} {abrev}",
                        FontSize = 11.5,
                        FontWeight = FontWeights.SemiBold,
                        ToolTip = $"{aloj.Codigo} · {dia:dd/MM/yyyy}\nEstado: {est}\nTarifa día: {DatosPrueba.ObtenerTarifaNoche(aloj, dia):C}"
                    };

                    Alojamiento alojCap = aloj;
                    DateTime diaCap = dia;
                    string estCap = est;
                    btn.Click += (s, ev) =>
                    {
                        if (estCap == "Libre")
                        {
                            var form = new ReservaFormWindow(null, alojCap, diaCap) { Owner = this };
                            if (form.ShowDialog() == true)
                            {
                                DatosPrueba.Reservas.Add(form.ReservaGuardada);
                                AplicarFiltrosReservas();
                                ConstruirCuadranteDisponibilidad();
                                InformarEstado($"Reserva {form.ReservaGuardada.Codigo} creada para {alojCap.Codigo}.");
                            }
                        }
                        else
                        {
                            MessageBox.Show(
                                $"Alojamiento: {alojCap.Codigo} ({alojCap.Tipo})\nFecha: {diaCap:dd/MM/yyyy}\nEstado: {estCap}",
                                "Información de disponibilidad",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                        }
                    };

                    Grid.SetRow(btn, r + 1);
                    Grid.SetColumn(btn, d + 1);
                    DisponibilidadCuadranteGrid.Children.Add(btn);
                }
            }
        }

        private static Border CrearCeldaCabecera(string texto, HorizontalAlignment align)
        {
            return new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(234, 239, 245)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(209, 216, 224)),
                BorderThickness = new Thickness(0, 0, 1, 1),
                Padding = new Thickness(6, 4, 6, 4),
                Child = new TextBlock
                {
                    Text = texto,
                    FontWeight = FontWeights.Bold,
                    FontSize = 11.5,
                    TextAlignment = align == HorizontalAlignment.Center ? TextAlignment.Center : TextAlignment.Left,
                    HorizontalAlignment = align,
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = new SolidColorBrush(Color.FromRgb(23, 50, 77))
                }
            };
        }

        private static string AbreviarEstado(string estado)
        {
            switch (estado)
            {
                case "Libre": return "Libre";
                case "Ocupada": return "Ocup.";
                case "Reservada": return "Reserv.";
                case "En reparación": return "Repar.";
                case "Pendiente de limpieza": return "Limp.";
                default: return estado;
            }
        }

        // ==================== PESTAÑA 3: RESERVAS ====================

        private void AplicarFiltrosReservas()
        {
            if (ReservasGrid == null) return;
            DateTime? desde = FiltroResDesdePicker.SelectedDate;
            DateTime? hasta = FiltroResHastaPicker.SelectedDate;
            string aloj = FiltroResAlojCombo.SelectedItem as string ?? "Todos";
            string busq = (FiltroResBuscarTextBox.Text ?? string.Empty).Trim().ToLowerInvariant();

            var filtradas = DatosPrueba.Reservas.Where(r =>
                (!desde.HasValue || r.Salida.Date >= desde.Value.Date) &&
                (!hasta.HasValue || r.Entrada.Date <= hasta.Value.Date) &&
                (aloj == "Todos" || r.Alojamiento == aloj) &&
                (string.IsNullOrEmpty(busq) ||
                 (r.Codigo ?? "").ToLowerInvariant().Contains(busq) ||
                 (r.Cliente ?? "").ToLowerInvariant().Contains(busq) ||
                 (r.Alojamiento ?? "").ToLowerInvariant().Contains(busq))).ToList();

            var sel = ReservasGrid.SelectedItem as Reserva;
            ReservasGrid.ItemsSource = filtradas;
            if (sel != null && filtradas.Contains(sel)) ReservasGrid.SelectedItem = sel;
            else if (filtradas.Count > 0) ReservasGrid.SelectedIndex = 0;

            ContadorResTextBlock.Text = $"{filtradas.Count} de {DatosPrueba.Reservas.Count} reservas";
            ActualizarDetalleReserva();
        }

        private void FiltroRes_DateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded) AplicarFiltrosReservas();
        }

        private void FiltroRes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded) AplicarFiltrosReservas();
        }

        private void FiltroRes_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsLoaded) AplicarFiltrosReservas();
        }

        private void LimpiarFiltrosReservas_Click(object sender, RoutedEventArgs e)
        {
            FiltroResDesdePicker.SelectedDate = null;
            FiltroResHastaPicker.SelectedDate = null;
            FiltroResAlojCombo.SelectedIndex = 0;
            FiltroResBuscarTextBox.Clear();
            AplicarFiltrosReservas();
        }

        private void ReservasGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ActualizarDetalleReserva();
        }

        private void ReservasGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ReservasGrid.SelectedItem is Reserva)
                ModificarReserva_Click(sender, e);
        }

        private void ActualizarDetalleReserva()
        {
            if (DetalleResTitulo == null) return;
            var r = ReservasGrid.SelectedItem as Reserva;
            if (r == null)
            {
                DetalleResTitulo.Text = "Seleccione una reserva para ver los detalles";
                DetalleResContacto.Text = string.Empty;
                DetalleResSolicitudes.Text = string.Empty;
                return;
            }

            DetalleResTitulo.Text = $"{r.Codigo} · {r.Cliente} · Alojamiento {r.Alojamiento} · {r.Noches} noches ({r.ImporteEstimado:C})";
            string emailTxt = string.IsNullOrWhiteSpace(r.Email) ? "Sin email" : r.Email;
            DetalleResContacto.Text = $"Contacto: Tel. {r.Telefonos} · {emailTxt} · Entrada est.: {r.HoraEntradaEstimada} · Salida est.: {r.HoraSalidaEstimada}";
            DetalleResSolicitudes.Text = $"Solicitudes especiales: {r.ResumenSolicitudes}";
        }

        private void AnadirReserva_Click(object sender, RoutedEventArgs e)
        {
            var form = new ReservaFormWindow { Owner = this };
            if (form.ShowDialog() == true)
            {
                DatosPrueba.Reservas.Add(form.ReservaGuardada);
                AplicarFiltrosReservas();
                ReservasGrid.SelectedItem = form.ReservaGuardada;
                ConstruirCuadranteDisponibilidad();
                InformarEstado($"Reserva {form.ReservaGuardada.Codigo} creada correctamente.");
            }
        }

        private void ModificarReserva_Click(object sender, RoutedEventArgs e)
        {
            if (!(ReservasGrid.SelectedItem is Reserva r))
            {
                MessageBox.Show("Seleccione primero una reserva.", "Selección necesaria",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var form = new ReservaFormWindow(r) { Owner = this };
            if (form.ShowDialog() == true)
            {
                AplicarFiltrosReservas();
                ConstruirCuadranteDisponibilidad();
                InformarEstado($"Reserva {r.Codigo} actualizada correctamente.");
            }
        }

        private void ReservaEnCurso_Click(object sender, RoutedEventArgs e)
        {
            CambiarEstadoReserva("En curso");
        }

        private void ReservaFinalizada_Click(object sender, RoutedEventArgs e)
        {
            CambiarEstadoReserva("Finalizada");
        }

        private void CambiarEstadoReserva(string nuevoEstado)
        {
            if (!(ReservasGrid.SelectedItem is Reserva r))
            {
                MessageBox.Show("Seleccione primero una reserva.", "Selección necesaria",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            r.Estado = nuevoEstado;
            AplicarFiltrosReservas();
            ConstruirCuadranteDisponibilidad();
            InformarEstado($"Reserva {r.Codigo} marcada como «{nuevoEstado}».");
        }

        private void EliminarReserva_Click(object sender, RoutedEventArgs e)
        {
            if (!(ReservasGrid.SelectedItem is Reserva r))
            {
                MessageBox.Show("Seleccione primero una reserva.", "Selección necesaria",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (ConfirmarEliminacion($"Reserva {r.Codigo} de {r.Cliente} ({r.Entrada:dd/MM/yyyy} - {r.Salida:dd/MM/yyyy})"))
            {
                DatosPrueba.Reservas.Remove(r);
                AplicarFiltrosReservas();
                ConstruirCuadranteDisponibilidad();
                InformarEstado($"Reserva {r.Codigo} cancelada y eliminada.");
            }
        }

        // ==================== PESTAÑA 4: EMPLEADOS ====================

        private void AplicarFiltrosEmpleados()
        {
            if (EmpleadosGrid == null) return;
            string funcion = (FiltroEmpFuncionCombo.SelectedItem as ComboBoxItem)?.Content as string ?? "Todas";
            string idioma = (FiltroEmpIdiomaCombo.SelectedItem as ComboBoxItem)?.Content as string ?? "Todos";
            string busq = (FiltroEmpBuscarTextBox.Text ?? string.Empty).Trim().ToLowerInvariant();

            var filtrados = DatosPrueba.Empleados.Where(emp =>
                (funcion == "Todas" || (emp.Tipo ?? "").ToLowerInvariant().Contains(funcion.ToLowerInvariant())) &&
                (idioma == "Todos" || (emp.Idiomas ?? "").ToLowerInvariant().Contains(idioma.ToLowerInvariant())) &&
                (string.IsNullOrEmpty(busq) ||
                 emp.NombreCompleto.ToLowerInvariant().Contains(busq) ||
                 (emp.Formacion ?? "").ToLowerInvariant().Contains(busq) ||
                 emp.Disponibilidad.ToLowerInvariant().Contains(busq))).ToList();

            var sel = EmpleadosGrid.SelectedItem as EmpleadoTemporal;
            EmpleadosGrid.ItemsSource = filtrados;
            if (sel != null && filtrados.Contains(sel)) EmpleadosGrid.SelectedItem = sel;
            else if (filtrados.Count > 0) EmpleadosGrid.SelectedIndex = 0;

            ContadorEmpTextBlock.Text = $"{filtrados.Count} de {DatosPrueba.Empleados.Count} empleados";
            ActualizarDetalleEmpleado();
        }

        private void FiltroEmpleados_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded) AplicarFiltrosEmpleados();
        }

        private void FiltroEmpleadosText_Changed(object sender, TextChangedEventArgs e)
        {
            if (IsLoaded) AplicarFiltrosEmpleados();
        }

        private void LimpiarFiltrosEmpleados_Click(object sender, RoutedEventArgs e)
        {
            FiltroEmpFuncionCombo.SelectedIndex = 0;
            FiltroEmpIdiomaCombo.SelectedIndex = 0;
            FiltroEmpBuscarTextBox.Clear();
            AplicarFiltrosEmpleados();
        }

        private void EmpleadosGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ActualizarDetalleEmpleado();
        }

        private void EmpleadosGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (EmpleadosGrid.SelectedItem is EmpleadoTemporal)
                ModificarEmpleado_Click(sender, e);
        }

        private void ActualizarDetalleEmpleado()
        {
            if (DetalleEmpNombre == null) return;
            var emp = EmpleadosGrid.SelectedItem as EmpleadoTemporal;
            if (emp == null)
            {
                DetalleEmpNombre.Text = "Seleccione un empleado";
                DetalleEmpFuncion.Text = string.Empty;
                DetalleEmpContacto.Text = string.Empty;
                DetalleEmpIdiomas.Text = string.Empty;
                DetalleEmpFormacion.Text = string.Empty;
                DetalleEmpDisponibilidad.Text = string.Empty;
                DetalleEmpActividades.Text = string.Empty;
                DetalleEmpFoto.Source = null;
                return;
            }

            DetalleEmpNombre.Text = emp.NombreCompleto;
            DetalleEmpFuncion.Text = emp.Tipo;
            DetalleEmpContacto.Text = $"Teléfonos: {emp.Telefono}\nCorreos: {emp.Email}";
            DetalleEmpIdiomas.Text = $"Idiomas: {emp.Idiomas}";
            DetalleEmpFormacion.Text = $"Formación: {(string.IsNullOrWhiteSpace(emp.Formacion) ? "No especificada" : emp.Formacion)}";
            DetalleEmpDisponibilidad.Text = $"Disponibilidad: {emp.Disponibilidad}";

            var conv = new ImagePathConverter();
            DetalleEmpFoto.Source = conv.Convert(emp.Foto, typeof(ImageSource), null, CultureInfo.CurrentCulture) as ImageSource;

            var acts = DatosPrueba.Actividades
                .Where(a => (a.Monitor ?? "").IndexOf(emp.NombreCompleto, StringComparison.OrdinalIgnoreCase) >= 0)
                .Select(a => $"• {a.Nombre} ({a.Horario} · {a.Zona})")
                .ToList();
            DetalleEmpActividades.Text = acts.Count > 0 ? string.Join("\n", acts) : "Sin actividades asignadas actualmente.";
        }

        private void AnadirEmpleado_Click(object sender, RoutedEventArgs e)
        {
            var form = new EmpleadoFormWindow { Owner = this };
            if (form.ShowDialog() == true)
            {
                DatosPrueba.Empleados.Add(form.EmpleadoGuardado);
                AplicarFiltrosEmpleados();
                EmpleadosGrid.SelectedItem = form.EmpleadoGuardado;
                InformarEstado($"Empleado «{form.EmpleadoGuardado.NombreCompleto}» dado de alta.");
            }
        }

        private void ModificarEmpleado_Click(object sender, RoutedEventArgs e)
        {
            if (!(EmpleadosGrid.SelectedItem is EmpleadoTemporal emp))
            {
                MessageBox.Show("Seleccione primero un empleado.", "Selección necesaria",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var form = new EmpleadoFormWindow(emp) { Owner = this };
            if (form.ShowDialog() == true)
            {
                AplicarFiltrosEmpleados();
                InformarEstado($"Ficha de «{emp.NombreCompleto}» actualizada.");
            }
        }

        private void EliminarEmpleado_Click(object sender, RoutedEventArgs e)
        {
            if (!(EmpleadosGrid.SelectedItem is EmpleadoTemporal emp))
            {
                MessageBox.Show("Seleccione primero un empleado.", "Selección necesaria",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            int numActs = DatosPrueba.Actividades.Count(a =>
                (a.Monitor ?? "").IndexOf(emp.NombreCompleto, StringComparison.OrdinalIgnoreCase) >= 0);
            string avisoActs = numActs > 0
                ? $"\n\n⚠ Atención: este empleado tiene {numActs} actividad(es) de verano asignada(s)."
                : string.Empty;

            if (ConfirmarEliminacion($"{emp.NombreCompleto} ({emp.Tipo}){avisoActs}"))
            {
                DatosPrueba.Empleados.Remove(emp);
                AplicarFiltrosEmpleados();
                InformarEstado($"Empleado «{emp.NombreCompleto}» dado de baja.");
            }
        }

        // ==================== PESTAÑA 5: ACTIVIDADES ====================

        private void AplicarFiltrosActividades()
        {
            if (ActividadesGrid == null) return;
            string zona = FiltroActZonaCombo.SelectedItem as string ?? "Todas";
            string dest = (FiltroActDestCombo.SelectedItem as ComboBoxItem)?.Content as string ?? "Todos";
            string dia = (FiltroActDiaCombo.SelectedItem as ComboBoxItem)?.Content as string ?? "Todos";
            string busq = (FiltroActBuscarTextBox.Text ?? string.Empty).Trim().ToLowerInvariant();

            var filtradas = DatosPrueba.Actividades.Where(a =>
                (zona == "Todas" || a.Zona == zona) &&
                (dest == "Todos" || a.Destinatarios == dest) &&
                (dia == "Todos" || (a.DiasSemana ?? "").Contains(dia)) &&
                (string.IsNullOrEmpty(busq) ||
                 (a.Nombre ?? "").ToLowerInvariant().Contains(busq) ||
                 (a.Monitor ?? "").ToLowerInvariant().Contains(busq) ||
                 (a.Zona ?? "").ToLowerInvariant().Contains(busq))).ToList();

            var sel = ActividadesGrid.SelectedItem as Actividad;
            ActividadesGrid.ItemsSource = filtradas;
            if (sel != null && filtradas.Contains(sel)) ActividadesGrid.SelectedItem = sel;
            else if (filtradas.Count > 0) ActividadesGrid.SelectedIndex = 0;

            ContadorActTextBlock.Text = $"{filtradas.Count} de {DatosPrueba.Actividades.Count} actividades";
            ActualizarDetalleActividad();
        }

        private void FiltroActividades_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded) AplicarFiltrosActividades();
        }

        private void FiltroActividadesText_Changed(object sender, TextChangedEventArgs e)
        {
            if (IsLoaded) AplicarFiltrosActividades();
        }

        private void LimpiarFiltrosActividades_Click(object sender, RoutedEventArgs e)
        {
            FiltroActZonaCombo.SelectedIndex = 0;
            FiltroActDestCombo.SelectedIndex = 0;
            FiltroActDiaCombo.SelectedIndex = 0;
            FiltroActBuscarTextBox.Clear();
            AplicarFiltrosActividades();
        }

        private void ActividadesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ActualizarDetalleActividad();
        }

        private void ActividadesGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ActividadesGrid.SelectedItem is Actividad)
                ModificarActividad_Click(sender, e);
        }

        private void ActualizarDetalleActividad()
        {
            if (DetalleActTitulo == null) return;
            var a = ActividadesGrid.SelectedItem as Actividad;
            if (a == null)
            {
                DetalleActTitulo.Text = "Seleccione una actividad para ver los detalles";
                DetalleActDescripcion.Text = string.Empty;
                DetalleActMateriales.Text = string.Empty;
                return;
            }

            DetalleActTitulo.Text = $"{a.Nombre} · {a.Horario} · Área: {a.Zona} · {a.PrecioResumen}";
            DetalleActDescripcion.Text = $"Descripción: {(string.IsNullOrWhiteSpace(a.Descripcion) ? "Sin descripción" : a.Descripcion)}";
            DetalleActMateriales.Text = $"Monitores: {a.Monitor} · Cupo: {a.CupoResumen} · Materiales: {(string.IsNullOrWhiteSpace(a.Materiales) ? "No requiere material específico" : a.Materiales)}";
        }

        private void AnadirActividad_Click(object sender, RoutedEventArgs e)
        {
            var form = new ActividadFormWindow { Owner = this };
            if (form.ShowDialog() == true)
            {
                DatosPrueba.Actividades.Add(form.ActividadGuardada);
                PoblarCombosDinamicos();
                AplicarFiltrosActividades();
                ActividadesGrid.SelectedItem = form.ActividadGuardada;
                ActualizarDetalleEmpleado();
                InformarEstado($"Actividad «{form.ActividadGuardada.Nombre}» creada correctamente.");
            }
        }

        private void ModificarActividad_Click(object sender, RoutedEventArgs e)
        {
            if (!(ActividadesGrid.SelectedItem is Actividad act))
            {
                MessageBox.Show("Seleccione primero una actividad.", "Selección necesaria",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var form = new ActividadFormWindow(act) { Owner = this };
            if (form.ShowDialog() == true)
            {
                PoblarCombosDinamicos();
                AplicarFiltrosActividades();
                ActualizarDetalleEmpleado();
                InformarEstado($"Actividad «{act.Nombre}» actualizada.");
            }
        }

        private void EliminarActividad_Click(object sender, RoutedEventArgs e)
        {
            if (!(ActividadesGrid.SelectedItem is Actividad act))
            {
                MessageBox.Show("Seleccione primero una actividad.", "Selección necesaria",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (ConfirmarEliminacion($"Actividad «{act.Nombre}» ({act.Horario})"))
            {
                DatosPrueba.Actividades.Remove(act);
                PoblarCombosDinamicos();
                AplicarFiltrosActividades();
                ActualizarDetalleEmpleado();
                InformarEstado($"Actividad «{act.Nombre}» eliminada.");
            }
        }

        private static bool ConfirmarEliminacion(string descripcion)
        {
            MessageBoxResult respuesta = MessageBox.Show(
                "¿Está seguro de que desea eliminar este registro?\nEsta acción no se puede deshacer.\n\n" + descripcion,
                "Confirmar eliminación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.No);

            return respuesta == MessageBoxResult.Yes;
        }
    }
}
