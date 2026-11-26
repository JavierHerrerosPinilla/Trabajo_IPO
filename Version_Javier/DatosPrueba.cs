using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace Trabajo_IPO
{
    public static class DatosPrueba
    {
        public static ObservableCollection<Gestor> Gestores { get; private set; }
        public static ObservableCollection<Alojamiento> Alojamientos { get; private set; }
        public static ObservableCollection<EmpleadoTemporal> Empleados { get; private set; }
        public static ObservableCollection<Reserva> Reservas { get; private set; }
        public static ObservableCollection<Actividad> Actividades { get; private set; }

        public static readonly string[] DiasSemanaOrden = { "Lun", "Mar", "Mié", "Jue", "Vie", "Sáb", "Dom" };

        static DatosPrueba()
        {
            Gestores = new ObservableCollection<Gestor>
            {
                new Gestor
                {
                    Usuario = "admin",
                    Contrasena = "camping2026",
                    NombreCompleto = "Laura García Martín",
                    Rol = "Gestora del complejo",
                    FotoPerfil = "pack://application:,,,/Assets/avatar_laura.png",
                    UltimoAcceso = new DateTime(2026, 10, 5, 18, 20, 0)
                },
                new Gestor
                {
                    Usuario = "javier",
                    Contrasena = "ipo2026",
                    NombreCompleto = "Javier Herreros Pinilla",
                    Rol = "Gestor del complejo",
                    FotoPerfil = "pack://application:,,,/Assets/avatar_javier.png",
                    UltimoAcceso = new DateTime(2026, 10, 4, 9, 10, 0)
                },
                new Gestor
                {
                    Usuario = "recepcion",
                    Contrasena = "bienvenido",
                    NombreCompleto = "María Sánchez Pérez",
                    Rol = "Recepción y gestión",
                    FotoPerfil = "pack://application:,,,/Assets/avatar_maria.png",
                    UltimoAcceso = new DateTime(2026, 10, 3, 12, 45, 0)
                }
            };

            Alojamientos = new ObservableCollection<Alojamiento>
            {
                new Alojamiento
                {
                    Codigo = "P-01",
                    Tipo = "Parcela",
                    Categoria = "Deluxe",
                    TamanoM2 = 45,
                    Ubicacion = "Junto a la piscina",
                    Capacidad = 6,
                    PrecioNoche = 38.50m,
                    PrecioTempBaja = 30.00m,
                    PrecioTempMedia = 38.50m,
                    PrecioTempAlta = 48.00m,
                    Estado = "Libre",
                    TieneAgua = true,
                    TieneLuz = true,
                    TieneDesague = true,
                    DelimitadaSetos = true,
                    ZonaSombra = true,
                    Descripcion = "Parcela amplia y muy sombreada junto a la zona de piscina y aseos centrales.",
                    Fotos = new ObservableCollection<string> { "pack://application:,,,/Assets/parcela_piscina.png" }
                },
                new Alojamiento
                {
                    Codigo = "P-07",
                    Tipo = "Parcela",
                    Categoria = "Pequeña",
                    TamanoM2 = 24,
                    Ubicacion = "Junto a aseos y fregaderos",
                    Capacidad = 2,
                    PrecioNoche = 18.00m,
                    PrecioTempBaja = 14.00m,
                    PrecioTempMedia = 18.00m,
                    PrecioTempAlta = 22.00m,
                    Estado = "Pendiente de limpieza",
                    TieneAgua = false,
                    TieneLuz = true,
                    TieneDesague = false,
                    DelimitadaSetos = false,
                    ZonaSombra = true,
                    Descripcion = "Parcela compacta ideal para tiendas de campaña y parejas.",
                    Fotos = new ObservableCollection<string> { "pack://application:,,,/Assets/parcela_piscina.png" }
                },
                new Alojamiento
                {
                    Codigo = "P-12",
                    Tipo = "Parcela",
                    Categoria = "Autocaravana",
                    TamanoM2 = 55,
                    Ubicacion = "Cerca de la entrada",
                    Capacidad = 6,
                    PrecioNoche = 32.00m,
                    PrecioTempBaja = 26.00m,
                    PrecioTempMedia = 32.00m,
                    PrecioTempAlta = 38.00m,
                    Estado = "Reservada",
                    TieneAgua = true,
                    TieneLuz = true,
                    TieneDesague = true,
                    DelimitadaSetos = true,
                    ZonaSombra = false,
                    Descripcion = "Acceso directo pavimentado para autocaravanas grandes, con toma de 16A y vaciado.",
                    Fotos = new ObservableCollection<string> { "pack://application:,,,/Assets/parcela_autocaravana.png" }
                },
                new Alojamiento
                {
                    Codigo = "P-24",
                    Tipo = "Parcela",
                    Categoria = "Mediana",
                    TamanoM2 = 34,
                    Ubicacion = "Zona tranquila, cerca de aseos",
                    Capacidad = 4,
                    PrecioNoche = 25.00m,
                    PrecioTempBaja = 20.00m,
                    PrecioTempMedia = 25.00m,
                    PrecioTempAlta = 32.00m,
                    Estado = "En reparación",
                    TieneAgua = true,
                    TieneLuz = false,
                    TieneDesague = false,
                    DelimitadaSetos = true,
                    ZonaSombra = true,
                    Descripcion = "Parcela familiar en revisión de acometida eléctrica.",
                    Fotos = new ObservableCollection<string> { "pack://application:,,,/Assets/parcela_piscina.png" }
                },
                new Alojamiento
                {
                    Codigo = "B-01",
                    Tipo = "Bungalow",
                    Categoria = "Bosque",
                    TamanoM2 = 52,
                    Ubicacion = "Zona arbolada norte",
                    Capacidad = 6,
                    PrecioNoche = 115.00m,
                    PrecioTempBaja = 115.00m,
                    PrecioTempMedia = 115.00m,
                    PrecioTempAlta = 115.00m,
                    EstanciaMinimaNoches = 3,
                    Estado = "Ocupada",
                    TieneMenaje = true,
                    TieneMicroondas = true,
                    TieneRopaCamaToallas = true,
                    TieneWifi = true,
                    TieneTerraza = true,
                    TieneAireAcondicionado = true,
                    Descripcion = "Cabaña de madera de dos dormitorios con amplio porche frente al pinar.",
                    Fotos = new ObservableCollection<string>
                    {
                        "pack://application:,,,/Assets/bungalow_bosque_1.png",
                        "pack://application:,,,/Assets/bungalow_familiar_2.png"
                    }
                },
                new Alojamiento
                {
                    Codigo = "B-02",
                    Tipo = "Bungalow",
                    Categoria = "Familiar",
                    TamanoM2 = 45,
                    Ubicacion = "Zona de bungalows",
                    Capacidad = 5,
                    PrecioNoche = 89.00m,
                    PrecioTempBaja = 89.00m,
                    PrecioTempMedia = 89.00m,
                    PrecioTempAlta = 89.00m,
                    EstanciaMinimaNoches = 2,
                    Estado = "Libre",
                    TieneMenaje = true,
                    TieneMicroondas = true,
                    TieneRopaCamaToallas = true,
                    TieneWifi = true,
                    TieneTerraza = true,
                    TieneAireAcondicionado = false,
                    Descripcion = "Bungalow familiar luminoso con cocina equipada, menaje completo y terraza cubierta.",
                    Fotos = new ObservableCollection<string>
                    {
                        "pack://application:,,,/Assets/bungalow_familiar_1.png",
                        "pack://application:,,,/Assets/bungalow_familiar_2.png"
                    }
                },
                new Alojamiento
                {
                    Codigo = "B-07",
                    Tipo = "Bungalow",
                    Categoria = "Pareja",
                    TamanoM2 = 32,
                    Ubicacion = "Junto al bar-restaurante",
                    Capacidad = 2,
                    PrecioNoche = 72.00m,
                    PrecioTempBaja = 72.00m,
                    PrecioTempMedia = 72.00m,
                    PrecioTempAlta = 72.00m,
                    EstanciaMinimaNoches = null,
                    Estado = "Libre",
                    TieneMenaje = true,
                    TieneMicroondas = true,
                    TieneRopaCamaToallas = true,
                    TieneWifi = true,
                    TieneTerraza = false,
                    TieneAireAcondicionado = true,
                    Descripcion = "Alojamiento acogedor para dos personas con climatización y acceso rápido a servicios.",
                    Fotos = new ObservableCollection<string>
                    {
                        "pack://application:,,,/Assets/bungalow_pareja_1.png",
                        "pack://application:,,,/Assets/bungalow_pareja_2.png"
                    }
                }
            };

            Empleados = new ObservableCollection<EmpleadoTemporal>
            {
                new EmpleadoTemporal
                {
                    Nombre = "Eva",
                    Apellidos = "Soto Díaz",
                    Foto = "pack://application:,,,/Assets/emp_eva.png",
                    Tipo = "Monitora",
                    Telefono = "600 111 222",
                    Email = "eva.soto@campingdemo.es",
                    Idiomas = "Español, Inglés",
                    Formacion = "Monitora de ocio y tiempo libre, primeros auxilios",
                    DiasNoDisponibles = "",
                    ObservacionesDisponibilidad = "Disponible todo el verano"
                },
                new EmpleadoTemporal
                {
                    Nombre = "Luis",
                    Apellidos = "Ortega Gil",
                    Foto = "pack://application:,,,/Assets/emp_luis.png",
                    Tipo = "Monitor",
                    Telefono = "600 333 444",
                    Email = "luis.ortega@campingdemo.es",
                    Idiomas = "Español, Francés",
                    Formacion = "Técnico deportivo en actividades al aire libre",
                    DiasNoDisponibles = "Mar",
                    ObservacionesDisponibilidad = "No disponible los martes por formación"
                },
                new EmpleadoTemporal
                {
                    Nombre = "Ana",
                    Apellidos = "Ruiz Mora",
                    Foto = "pack://application:,,,/Assets/emp_ana.png",
                    Tipo = "Guía",
                    Telefono = "600 555 666",
                    Email = "ana.ruiz@campingdemo.es",
                    Idiomas = "Español, Inglés, Alemán",
                    Formacion = "Guía de montaña e interpretación ambiental",
                    DiasNoDisponibles = "Sáb, Dom",
                    ObservacionesDisponibilidad = "Disponible de lunes a viernes"
                },
                new EmpleadoTemporal
                {
                    Nombre = "Pablo",
                    Apellidos = "Vega Ríos",
                    Foto = "pack://application:,,,/Assets/emp_pablo.png",
                    Tipo = "Monitor",
                    Telefono = "600 777 888",
                    Email = "pablo.vega@campingdemo.es",
                    Idiomas = "Español",
                    Formacion = "Socorrista acuático y monitor infantil",
                    DiasNoDisponibles = "",
                    ObservacionesDisponibilidad = "Turno de mañanas y fines de semana"
                },
                new EmpleadoTemporal
                {
                    Nombre = "Carla",
                    Apellidos = "Nieto Pérez",
                    Foto = "pack://application:,,,/Assets/emp_carla.png",
                    Tipo = "Guía y monitora",
                    Telefono = "600 999 000, 926 221 100",
                    Email = "carla.nieto@campingdemo.es",
                    Idiomas = "Español, Inglés, Francés",
                    Formacion = "Grado en CAFD y dinamización turística",
                    DiasNoDisponibles = "",
                    ObservacionesDisponibilidad = "Turno completo todo el verano"
                }
            };

            Reservas = new ObservableCollection<Reserva>
            {
                new Reserva
                {
                    Codigo = "R-1001",
                    Alojamiento = "P-12",
                    Cliente = "Ana Ruiz Gómez",
                    Telefonos = "611 234 567",
                    Email = "ana.ruiz@correo.es",
                    Entrada = new DateTime(2026, 7, 10),
                    Salida = new DateTime(2026, 7, 15),
                    HoraEntradaEstimada = "16:00",
                    HoraSalidaEstimada = "11:00",
                    Ocupantes = 4,
                    Estado = "Confirmada",
                    SolicitudesEspeciales = "Acceso cercano a aseos",
                    ImporteEstimado = 190.00m
                },
                new Reserva
                {
                    Codigo = "R-1002",
                    Alojamiento = "B-02",
                    Cliente = "Carlos Pérez Soto",
                    Telefonos = "622 345 678",
                    Email = "carlos.perez@correo.es",
                    Entrada = new DateTime(2026, 8, 1),
                    Salida = new DateTime(2026, 8, 8),
                    HoraEntradaEstimada = "17:30",
                    HoraSalidaEstimada = "10:30",
                    Ocupantes = 5,
                    Estado = "Pendiente",
                    NecesitaCuna = true,
                    NecesitaMenaje = true,
                    SolicitudesEspeciales = "Llegada con bebé",
                    ImporteEstimado = 623.00m
                },
                new Reserva
                {
                    Codigo = "R-1003",
                    Alojamiento = "B-01",
                    Cliente = "Marta López Díaz",
                    Telefonos = "633 456 789, 910 223 344",
                    Email = "marta.lopez@correo.es",
                    Entrada = new DateTime(2026, 7, 1),
                    Salida = new DateTime(2026, 7, 6),
                    HoraEntradaEstimada = "15:00",
                    HoraSalidaEstimada = "11:00",
                    Ocupantes = 6,
                    Estado = "En curso",
                    SolicitudesEspeciales = "Plaza de aparcamiento próxima",
                    ImporteEstimado = 575.00m
                },
                new Reserva
                {
                    Codigo = "R-1004",
                    Alojamiento = "P-01",
                    Cliente = "Jorge Ríos Mena",
                    Telefonos = "644 567 890",
                    Email = "",
                    Entrada = new DateTime(2026, 7, 1),
                    Salida = new DateTime(2026, 7, 5),
                    HoraEntradaEstimada = "13:00",
                    HoraSalidaEstimada = "12:00",
                    Ocupantes = 4,
                    Estado = "En curso",
                    SolicitudesEspeciales = "",
                    ImporteEstimado = 192.00m
                },
                new Reserva
                {
                    Codigo = "R-1005",
                    Alojamiento = "P-01",
                    Cliente = "Elena Navarro Gil",
                    Telefonos = "655 112 233",
                    Email = "elena.navarro@correo.es",
                    Entrada = new DateTime(2026, 7, 8),
                    Salida = new DateTime(2026, 7, 13),
                    HoraEntradaEstimada = "16:30",
                    HoraSalidaEstimada = "11:00",
                    Ocupantes = 3,
                    Estado = "Confirmada",
                    MovilidadReducida = true,
                    SolicitudesEspeciales = "Parcela llana cerca de rampa",
                    ImporteEstimado = 240.00m
                },
                new Reserva
                {
                    Codigo = "R-1006",
                    Alojamiento = "P-07",
                    Cliente = "Sonia Vera Cruz",
                    Telefonos = "666 789 012",
                    Email = "",
                    Entrada = new DateTime(2026, 6, 15),
                    Salida = new DateTime(2026, 6, 18),
                    HoraEntradaEstimada = "18:00",
                    HoraSalidaEstimada = "11:00",
                    Ocupantes = 2,
                    Estado = "Finalizada",
                    SolicitudesEspeciales = "",
                    ImporteEstimado = 54.00m
                }
            };

            Actividades = new ObservableCollection<Actividad>
            {
                new Actividad
                {
                    Nombre = "Tiro con arco",
                    Descripcion = "Iniciación al tiro con arco recreativo con material homologado y supervisión.",
                    Monitor = "Luis Ortega Gil",
                    DiasSemana = "Lun, Mié, Vie",
                    HoraInicio = "10:00",
                    HoraFin = "11:00",
                    Destinatarios = "Todos",
                    CupoMinimo = 4,
                    CupoMaximo = 12,
                    PrecioHora = 8.00m,
                    PrecioMes = null,
                    Zona = "Pista deportiva",
                    Materiales = "Arcos de iniciación, flechas de seguridad, dianas y protectores"
                },
                new Actividad
                {
                    Nombre = "Yoga al amanecer",
                    Descripcion = "Sesión guiada de respiración y estiramientos al aire libre.",
                    Monitor = "Ana Ruiz Mora",
                    DiasSemana = "Mar, Jue",
                    HoraInicio = "08:00",
                    HoraFin = "09:00",
                    Destinatarios = "Adultos",
                    CupoMinimo = 5,
                    CupoMaximo = 15,
                    PrecioHora = null,
                    PrecioMes = 30.00m,
                    Zona = "Zona verde",
                    Materiales = "Esterillas antideslizantes y bloques de apoyo"
                },
                new Actividad
                {
                    Nombre = "Senderismo guiado",
                    Descripcion = "Ruta interpretativa por el sendero norte y mirador del pinar.",
                    Monitor = "Carla Nieto Pérez, Luis Ortega Gil",
                    DiasSemana = "Sáb",
                    HoraInicio = "09:00",
                    HoraFin = "13:00",
                    Destinatarios = "Todos",
                    CupoMinimo = 6,
                    CupoMaximo = 20,
                    PrecioHora = 12.00m,
                    PrecioMes = null,
                    Zona = "Entrada y sendero norte",
                    Materiales = "Bastones de trekking, botiquín y prismáticos"
                },
                new Actividad
                {
                    Nombre = "Taller de manualidades",
                    Descripcion = "Creación de recuerdos con elementos naturales recogidos en el complejo.",
                    Monitor = "Eva Soto Díaz",
                    DiasSemana = "Lun, Mar, Mié, Jue, Vie",
                    HoraInicio = "17:00",
                    HoraFin = "18:00",
                    Destinatarios = "Niños",
                    CupoMinimo = 4,
                    CupoMaximo = 10,
                    PrecioHora = 6.50m,
                    PrecioMes = 25.00m,
                    Zona = "Sala común infantil",
                    Materiales = "Cartulinas, pinturas al agua, tijeras escolares y pegamento"
                },
                new Actividad
                {
                    Nombre = "Natación infantil",
                    Descripcion = "Juegos acuáticos y perfeccionamiento básico en piscina supervisada.",
                    Monitor = "Pablo Vega Ríos",
                    DiasSemana = "Lun, Mié, Vie",
                    HoraInicio = "11:30",
                    HoraFin = "12:30",
                    Destinatarios = "Niños",
                    CupoMinimo = 3,
                    CupoMaximo = 12,
                    PrecioHora = 10.00m,
                    PrecioMes = 35.00m,
                    Zona = "Piscina",
                    Materiales = "Tablas de natación, manguitos y material flotante"
                }
            };
        }

        public static string CalcularTemporada(DateTime fecha)
        {
            int mes = fecha.Month;
            if (mes == 7 || mes == 8) return "Alta";
            if (mes == 6 || mes == 9 || mes == 4) return "Media";
            return "Baja";
        }

        public static decimal ObtenerTarifaNoche(Alojamiento alojamiento, DateTime fecha)
        {
            if (alojamiento == null) return 0m;
            if (alojamiento.Tipo != "Parcela") return alojamiento.PrecioNoche;

            string temp = CalcularTemporada(fecha);
            switch (temp)
            {
                case "Alta":
                    return alojamiento.PrecioTempAlta > 0 ? alojamiento.PrecioTempAlta : alojamiento.PrecioNoche;
                case "Media":
                    return alojamiento.PrecioTempMedia > 0 ? alojamiento.PrecioTempMedia : alojamiento.PrecioNoche;
                default:
                    return alojamiento.PrecioTempBaja > 0 ? alojamiento.PrecioTempBaja : alojamiento.PrecioNoche;
            }
        }

        public static decimal CalcularImporteReserva(Alojamiento alojamiento, DateTime entrada, DateTime salida)
        {
            if (alojamiento == null || salida.Date <= entrada.Date) return 0m;
            decimal total = 0m;
            for (DateTime d = entrada.Date; d < salida.Date; d = d.AddDays(1))
            {
                total += ObtenerTarifaNoche(alojamiento, d);
            }
            return total;
        }

        public static string ObtenerEstadoAlojamientoEnFecha(Alojamiento alojamiento, DateTime fecha)
        {
            if (alojamiento == null) return "Libre";
            if (alojamiento.Estado == "En reparación") return "En reparación";

            var reserva = Reservas.FirstOrDefault(r =>
                r.Alojamiento == alojamiento.Codigo &&
                r.Estado != "Cancelada" &&
                r.Estado != "Finalizada" &&
                fecha.Date >= r.Entrada.Date &&
                fecha.Date < r.Salida.Date);

            if (reserva != null)
            {
                return reserva.Estado == "En curso" ? "Ocupada" : "Reservada";
            }

            if (fecha.Date == DateTime.Today && alojamiento.Estado == "Pendiente de limpieza")
            {
                return "Pendiente de limpieza";
            }

            return "Libre";
        }

        public static List<string> DetectarConflictosMonitor(
            string nombreMonitor,
            string diasSemana,
            string horaInicio,
            string horaFin,
            Actividad excluir = null)
        {
            var avisos = new List<string>();
            if (string.IsNullOrWhiteSpace(nombreMonitor) || string.IsNullOrWhiteSpace(diasSemana))
                return avisos;

            var diasActividad = diasSemana.Split(',').Select(d => d.Trim()).Where(d => d.Length > 0).ToList();
            var emp = Empleados.FirstOrDefault(e => e.NombreCompleto.Equals(nombreMonitor, StringComparison.OrdinalIgnoreCase));
            if (emp != null)
            {
                var diasNoDisp = emp.ListaDiasNoDisponibles.Intersect(diasActividad, StringComparer.OrdinalIgnoreCase).ToList();
                if (diasNoDisp.Count > 0)
                {
                    avisos.Add($"{nombreMonitor}: no disponible los días {string.Join(", ", diasNoDisp)}.");
                }
            }

            TimeSpan iniA, finA;
            if (!TimeSpan.TryParseExact(horaInicio, @"hh\:mm", CultureInfo.InvariantCulture, out iniA) ||
                !TimeSpan.TryParseExact(horaFin, @"hh\:mm", CultureInfo.InvariantCulture, out finA))
            {
                return avisos;
            }

            foreach (var act in Actividades)
            {
                if (act == excluir) continue;
                var monitoresAct = (act.Monitor ?? string.Empty).Split(',').Select(m => m.Trim());
                if (!monitoresAct.Contains(nombreMonitor, StringComparer.OrdinalIgnoreCase)) continue;

                var diasExist = (act.DiasSemana ?? string.Empty).Split(',').Select(d => d.Trim());
                var diasComunes = diasActividad.Intersect(diasExist, StringComparer.OrdinalIgnoreCase).ToList();
                if (diasComunes.Count == 0) continue;

                TimeSpan iniB, finB;
                if (TimeSpan.TryParseExact(act.HoraInicio, @"hh\:mm", CultureInfo.InvariantCulture, out iniB) &&
                    TimeSpan.TryParseExact(act.HoraFin, @"hh\:mm", CultureInfo.InvariantCulture, out finB))
                {
                    if (iniA < finB && finA > iniB)
                    {
                        avisos.Add($"{nombreMonitor}: solape con «{act.Nombre}» ({string.Join(", ", diasComunes)} {act.HoraInicio}-{act.HoraFin}).");
                    }
                }
            }

            return avisos;
        }
    }
}
