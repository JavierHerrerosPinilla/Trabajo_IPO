using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Trabajo_IPO
{
    /// <summary>
    /// Convierte un estado en un color de fondo suave consistente con la paleta IPO.
    /// Se combina siempre con icono y texto para garantizar accesibilidad ante daltonismo.
    /// </summary>
    public class EstadoToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string estado = (value as string ?? string.Empty).Trim();
            switch (estado)
            {
                case "Libre":
                case "En curso":
                    return new SolidColorBrush(Color.FromRgb(230, 244, 234));
                case "Reservada":
                case "Confirmada":
                    return new SolidColorBrush(Color.FromRgb(232, 241, 250));
                case "En reparación":
                case "Pendiente":
                    return new SolidColorBrush(Color.FromRgb(255, 244, 229));
                case "Pendiente de limpieza":
                    return new SolidColorBrush(Color.FromRgb(243, 232, 253));
                case "Ocupada":
                case "Cancelada":
                    return new SolidColorBrush(Color.FromRgb(253, 236, 234));
                case "Finalizada":
                    return new SolidColorBrush(Color.FromRgb(234, 239, 245));
                default:
                    return new SolidColorBrush(Color.FromRgb(240, 242, 245));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class EstadoToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string estado = (value as string ?? string.Empty).Trim();
            switch (estado)
            {
                case "Libre":
                case "En curso":
                    return new SolidColorBrush(Color.FromRgb(30, 126, 52));
                case "Reservada":
                case "Confirmada":
                    return new SolidColorBrush(Color.FromRgb(21, 101, 192));
                case "En reparación":
                case "Pendiente":
                    return new SolidColorBrush(Color.FromRgb(178, 106, 0));
                case "Pendiente de limpieza":
                    return new SolidColorBrush(Color.FromRgb(106, 27, 154));
                case "Ocupada":
                case "Cancelada":
                    return new SolidColorBrush(Color.FromRgb(198, 40, 40));
                case "Finalizada":
                    return new SolidColorBrush(Color.FromRgb(23, 50, 77));
                default:
                    return new SolidColorBrush(Color.FromRgb(82, 97, 107));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// Proporciona un icono Unicode legible junto al texto del estado (codificación redundante).
    /// </summary>
    public class EstadoToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string estado = (value as string ?? string.Empty).Trim();
            switch (estado)
            {
                case "Libre":
                case "Finalizada":
                    return "✓";
                case "En curso":
                    return "▶";
                case "Reservada":
                case "Confirmada":
                    return "◷";
                case "En reparación":
                case "Pendiente":
                    return "⚠";
                case "Pendiente de limpieza":
                    return "✦";
                case "Ocupada":
                case "Cancelada":
                    return "✕";
                default:
                    return "•";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// Carga segura de imágenes desde recursos embebidos pack:// o rutas locales de disco.
    /// </summary>
    public class ImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = value as string;
            if (string.IsNullOrWhiteSpace(path))
            {
                return null;
            }

            try
            {
                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                if (path.StartsWith("pack://", StringComparison.OrdinalIgnoreCase))
                {
                    const string prefix = "pack://application:,,,/";
                    string rel = path.Substring(prefix.Length).TrimStart('/');
                    Uri asmPackUri = new Uri("pack://application:,,,/Trabajo_IPO;component/" + rel, UriKind.Absolute);
                    try
                    {
                        var info = Application.GetResourceStream(asmPackUri) ?? Application.GetResourceStream(new Uri(path, UriKind.Absolute));
                        if (info != null)
                        {
                            bmp.StreamSource = info.Stream;
                        }
                    }
                    catch
                    {
                        // Fallback a archivo en disco junto al ensamblado
                    }

                    if (bmp.StreamSource == null)
                    {
                        string asmDir = Path.GetDirectoryName(typeof(ImagePathConverter).Assembly.Location) ?? AppDomain.CurrentDomain.BaseDirectory;
                        string local = Path.Combine(asmDir, rel.Replace('/', Path.DirectorySeparatorChar));
                        if (!File.Exists(local))
                        {
                            local = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rel.Replace('/', Path.DirectorySeparatorChar));
                        }
                        if (!File.Exists(local)) return null;
                        bmp.UriSource = new Uri(Path.GetFullPath(local), UriKind.Absolute);
                    }
                }
                else if (File.Exists(path))
                {
                    bmp.UriSource = new Uri(Path.GetFullPath(path), UriKind.Absolute);
                }
                else
                {
                    return null;
                }
                bmp.EndInit();
                bmp.Freeze();
                return bmp;
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class NullOrEmptyToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool invert = (parameter as string) == "Invert";
            bool hasValue = value != null && !string.IsNullOrWhiteSpace(value.ToString());
            if (value is int count)
            {
                hasValue = count > 0;
            }
            bool visible = invert ? !hasValue : hasValue;
            return visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
