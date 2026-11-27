using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Trabajo_IPO
{
    /// <summary>
    /// Clase base que notifica cambios a la interfaz WPF para actualización dinámica.
    /// </summary>
    public class NotifiableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Gestor : NotifiableBase
    {
        private DateTime ultimoAcceso;

        public string Usuario { get; set; }
        public string NombreCompleto { get; set; }
        public string Rol { get; set; }
        public string Contrasena { get; set; }
        public string FotoPerfil { get; set; }

        public DateTime UltimoAcceso
        {
            get => ultimoAcceso;
            set
            {
                ultimoAcceso = value;
                OnPropertyChanged();
            }
        }
    }

    public class Alojamiento : NotifiableBase
    {
        private string estado;

        public string Codigo { get; set; }
        public string Tipo { get; set; } // Parcela | Bungalow
        public string Categoria { get; set; }
        public double TamanoM2 { get; set; }
        public string Ubicacion { get; set; }
        public int Capacidad { get; set; }

        // Tarifa base (Bungalows) y tarifas por temporada (Parcelas)
        public decimal PrecioNoche { get; set; }
        public decimal PrecioTempBaja { get; set; }
        public decimal PrecioTempMedia { get; set; }
        public decimal PrecioTempAlta { get; set; }

        public int? EstanciaMinimaNoches { get; set; }

        public string Estado
        {
            get => estado;
            set
            {
                estado = value;
                OnPropertyChanged();
            }
        }

        // Servicios específicos de Parcela
        public bool TieneAgua { get; set; }
        public bool TieneLuz { get; set; }
        public bool TieneDesague { get; set; }
        public bool DelimitadaSetos { get; set; }
        public bool ZonaSombra { get; set; }

        // Equipamiento específico de Bungalow
        public bool TieneMenaje { get; set; }
        public bool TieneMicroondas { get; set; }
        public bool TieneRopaCamaToallas { get; set; }
        public bool TieneWifi { get; set; }
        public bool TieneTerraza { get; set; }
        public bool TieneAireAcondicionado { get; set; }

        public string Descripcion { get; set; }
        public ObservableCollection<string> Fotos { get; set; } = new ObservableCollection<string>();

        public string TamanoTexto => TamanoM2 > 0 ? string.Format(CultureInfo.CurrentCulture, "{0:0.#} m²", TamanoM2) : "-";

        public string ResumenPrecio
        {
            get
            {
                if (Tipo == "Parcela")
                {
                    return string.Format(CultureInfo.CurrentCulture, "{0:0.##} - {1:0.##} €", PrecioTempBaja, PrecioTempAlta);
                }
                return string.Format(CultureInfo.CurrentCulture, "{0:0.##} €", PrecioNoche);
            }
        }

        public string TarifasDetalle
        {
            get
            {
                if (Tipo == "Parcela")
                {
                    return string.Format(
                        CultureInfo.CurrentCulture,
                        "Temporada baja: {0:C} · Media: {1:C} · Alta: {2:C}",
                        PrecioTempBaja, PrecioTempMedia, PrecioTempAlta);
                }

                string minTxt = EstanciaMinimaNoches.HasValue && EstanciaMinimaNoches.Value > 0
                    ? $" · Estancia mínima: {EstanciaMinimaNoches.Value} noches"
                    : " · Sin estancia mínima";
                return string.Format(CultureInfo.CurrentCulture, "Tarifa fija: {0:C}/noche{1}", PrecioNoche, minTxt);
            }
        }

        public string Caracteristicas
        {
            get
            {
                var items = new List<string>();
                if (Tipo == "Parcela")
                {
                    if (TieneAgua) items.Add("Toma de agua");
                    if (TieneLuz) items.Add("Toma de luz");
                    if (TieneDesague) items.Add("Desagüe");
                    if (DelimitadaSetos) items.Add("Delimitada por setos");
                    if (ZonaSombra) items.Add("Zona de sombra");
                }
                else
                {
                    if (TieneMenaje) items.Add("Menaje");
                    if (TieneMicroondas) items.Add("Microondas");
                    if (TieneRopaCamaToallas) items.Add("Ropa de cama y toallas");
                    if (TieneWifi) items.Add("Wi-Fi");
                    if (TieneTerraza) items.Add("Terraza");
                    if (TieneAireAcondicionado) items.Add("Aire acondicionado");
                }
                return items.Count > 0 ? string.Join(", ", items) : "Equipamiento básico";
            }
        }

        public string FotoPrincipal => Fotos != null && Fotos.Count > 0 ? Fotos[0] : string.Empty;

        public string EtiquetaCombo => $"{Codigo} · {Tipo} {Categoria} (máx. {Capacidad} pers.)";

        public void RefrescarPropiedadesCalculadas()
        {
            OnPropertyChanged(string.Empty);
        }
    }

    public class Reserva : NotifiableBase
    {
        private string estado;

        public string Codigo { get; set; }
        public string Alojamiento { get; set; }
        public string Cliente { get; set; }
        public string Telefonos { get; set; }
        public string Email { get; set; }
        public DateTime Entrada { get; set; }
        public DateTime Salida { get; set; }
        public string HoraEntradaEstimada { get; set; }
        public string HoraSalidaEstimada { get; set; }
        public int Ocupantes { get; set; }

        public string Estado
        {
            get => estado;
            set
            {
                estado = value;
                OnPropertyChanged();
            }
        }

        public bool NecesitaCuna { get; set; }
        public bool NecesitaMenaje { get; set; }
        public bool MovilidadReducida { get; set; }
        public string SolicitudesEspeciales { get; set; }
        public decimal ImporteEstimado { get; set; }

        public int Noches => Math.Max(0, (Salida.Date - Entrada.Date).Days);

        public string ResumenSolicitudes
        {
            get
            {
                var tags = new List<string>();
                if (NecesitaCuna) tags.Add("Cuna");
                if (NecesitaMenaje) tags.Add("Menaje concreto");
                if (MovilidadReducida) tags.Add("Movilidad reducida");
                if (!string.IsNullOrWhiteSpace(SolicitudesEspeciales))
                {
                    tags.Add(SolicitudesEspeciales.Trim());
                }
                return tags.Count > 0 ? string.Join(" · ", tags) : "Sin solicitudes especiales";
            }
        }

        public void RefrescarPropiedadesCalculadas()
        {
            OnPropertyChanged(string.Empty);
        }
    }

    public class EmpleadoTemporal : NotifiableBase
    {
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Foto { get; set; }
        public string Tipo { get; set; } // Guía | Monitor | Guía y monitor
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Idiomas { get; set; }
        public string Formacion { get; set; }
        public string DiasNoDisponibles { get; set; } // ej. "Mar, Jue"
        public string ObservacionesDisponibilidad { get; set; }

        public string NombreCompleto => $"{Nombre} {Apellidos}".Trim();

        public string Disponibilidad
        {
            get
            {
                if (string.IsNullOrWhiteSpace(DiasNoDisponibles))
                {
                    return string.IsNullOrWhiteSpace(ObservacionesDisponibilidad)
                        ? "Disponible todo el verano"
                        : ObservacionesDisponibilidad;
                }
                string baseTxt = "No disp.: " + DiasNoDisponibles;
                return string.IsNullOrWhiteSpace(ObservacionesDisponibilidad)
                    ? baseTxt
                    : $"{baseTxt} ({ObservacionesDisponibilidad})";
            }
        }

        public List<string> ListaDiasNoDisponibles =>
            string.IsNullOrWhiteSpace(DiasNoDisponibles)
                ? new List<string>()
                : DiasNoDisponibles.Split(',').Select(d => d.Trim()).Where(d => d.Length > 0).ToList();

        public void RefrescarPropiedadesCalculadas()
        {
            OnPropertyChanged(string.Empty);
        }
    }

    public class Actividad : NotifiableBase
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Monitor { get; set; } // Nombres separados por coma
        public string DiasSemana { get; set; } // ej. "Lun, Mar, Mié, Jue, Vie"
        public string HoraInicio { get; set; } // "10:00"
        public string HoraFin { get; set; }    // "11:00"
        public string Destinatarios { get; set; } // Niños | Adultos | Todos
        public int CupoMinimo { get; set; }
        public int CupoMaximo { get; set; }
        public decimal? PrecioHora { get; set; }
        public decimal? PrecioMes { get; set; }
        public string Zona { get; set; }
        public string Materiales { get; set; }

        public string Horario => $"{DiasSemana} · {HoraInicio}-{HoraFin}";

        public string CupoResumen => $"{CupoMinimo} - {CupoMaximo} pers.";

        public string PrecioResumen
        {
            get
            {
                var p = new List<string>();
                if (PrecioHora.HasValue && PrecioHora.Value > 0)
                    p.Add(string.Format(CultureInfo.CurrentCulture, "{0:0.##} €/h", PrecioHora.Value));
                if (PrecioMes.HasValue && PrecioMes.Value > 0)
                    p.Add(string.Format(CultureInfo.CurrentCulture, "{0:0.##} €/mes", PrecioMes.Value));
                return p.Count > 0 ? string.Join(" / ", p) : "Gratuita";
            }
        }

        public void RefrescarPropiedadesCalculadas()
        {
            OnPropertyChanged(string.Empty);
        }
    }
}
