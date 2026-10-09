using System;
using System.Collections.ObjectModel;

namespace Trabajo_IPO
{
    public static class DatosPrueba
    {
        public static ObservableCollection<Gestor> Gestores { get; private set; }
        public static ObservableCollection<Alojamiento> Alojamientos { get; private set; }
        public static ObservableCollection<EmpleadoTemporal> Empleados { get; private set; }
        public static ObservableCollection<Reserva> Reservas { get; private set; }
        public static ObservableCollection<Actividad> Actividades { get; private set; }

        static DatosPrueba()
        {
            Gestores = new ObservableCollection<Gestor>
            {
                new Gestor { Usuario = "admin", Contrasena = "camping2026", NombreCompleto = "Laura García Martín", Rol = "Gestora del complejo", FotoPerfil = "", UltimoAcceso = new DateTime(2026, 10, 5, 18, 20, 0) },
                new Gestor { Usuario = "javier", Contrasena = "ipo2026", NombreCompleto = "Javier López Ruiz", Rol = "Gestor del complejo", FotoPerfil = "", UltimoAcceso = new DateTime(2026, 10, 4, 9, 10, 0) },
                new Gestor { Usuario = "recepcion", Contrasena = "bienvenido", NombreCompleto = "María Sánchez Pérez", Rol = "Recepción", FotoPerfil = "", UltimoAcceso = new DateTime(2026, 10, 3, 12, 45, 0) }
            };

            Alojamientos = new ObservableCollection<Alojamiento>
            {
                new Alojamiento { Codigo = "P-01", Tipo = "Parcela", Categoria = "Deluxe", Ubicacion = "Junto a la piscina", Capacidad = 6, PrecioNoche = 38.50m, Estado = "Libre", Caracteristicas = "Agua, luz, desagüe y sombra" },
                new Alojamiento { Codigo = "P-12", Tipo = "Parcela", Categoria = "Autocaravana", Ubicacion = "Cerca de la entrada", Capacidad = 6, PrecioNoche = 32.00m, Estado = "Reservado", Caracteristicas = "Luz, desagüe y acceso amplio" },
                new Alojamiento { Codigo = "P-24", Tipo = "Parcela", Categoria = "Mediana", Ubicacion = "Zona tranquila, cerca de aseos", Capacidad = 4, PrecioNoche = 25.00m, Estado = "En reparación", Caracteristicas = "Agua y sombra parcial" },
                new Alojamiento { Codigo = "B-02", Tipo = "Bungalow", Categoria = "Familiar", Ubicacion = "Zona de bungalows", Capacidad = 5, PrecioNoche = 89.00m, Estado = "Libre", Caracteristicas = "Wi-Fi, cocina, menaje, ropa de cama y terraza" },
                new Alojamiento { Codigo = "B-07", Tipo = "Bungalow", Categoria = "Pareja", Ubicacion = "Junto al bar-restaurante", Capacidad = 2, PrecioNoche = 72.00m, Estado = "Pendiente de limpieza", Caracteristicas = "Wi-Fi, microondas, toallas y aire acondicionado" }
            };

            Empleados = new ObservableCollection<EmpleadoTemporal>
            {
                new EmpleadoTemporal { NombreCompleto = "Nora Martín Vidal", Tipo = "Monitora", Telefono = "600 111 222", Email = "nora@campingdemo.es", Idiomas = "Español, inglés", Formacion = "Monitor de ocio y tiempo libre", Disponibilidad = "Mañanas y fines de semana" },
                new EmpleadoTemporal { NombreCompleto = "Álvaro Torres Gil", Tipo = "Guía y monitor", Telefono = "600 333 444", Email = "alvaro@campingdemo.es", Idiomas = "Español, francés", Formacion = "Técnico en actividades físicas", Disponibilidad = "Turno completo" }
            };

            Reservas = new ObservableCollection<Reserva>
            {
                new Reserva { Codigo = "R-1001", Alojamiento = "P-12", Cliente = "Ana Ruiz Gómez", Entrada = new DateTime(2026, 7, 10), Salida = new DateTime(2026, 7, 15), Ocupantes = 4, Estado = "Confirmada", SolicitudesEspeciales = "Acceso cercano a aseos" },
                new Reserva { Codigo = "R-1002", Alojamiento = "B-02", Cliente = "Carlos Pérez Soto", Entrada = new DateTime(2026, 8, 1), Salida = new DateTime(2026, 8, 8), Ocupantes = 5, Estado = "Pendiente", SolicitudesEspeciales = "Cuna y menaje infantil" }
            };

            Actividades = new ObservableCollection<Actividad>
            {
                new Actividad { Nombre = "Ruta familiar", Monitor = "Álvaro Torres Gil", Horario = "Lunes y miércoles, 10:00", Destinatarios = "Familias", CupoMaximo = 15, PrecioHora = 8.00m, Zona = "Entrada y sendero norte" },
                new Actividad { Nombre = "Taller de naturaleza", Monitor = "Nora Martín Vidal", Horario = "Martes, 17:00", Destinatarios = "Niños", CupoMaximo = 12, PrecioHora = 6.50m, Zona = "Zona infantil" }
            };
        }
    }
}
