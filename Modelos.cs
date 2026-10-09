using System;

namespace Trabajo_IPO
{
    public class Gestor
    {
        public string Usuario { get; set; }
        public string NombreCompleto { get; set; }
        public string Rol { get; set; }
        public string Contrasena { get; set; }
        public string FotoPerfil { get; set; }
        public DateTime UltimoAcceso { get; set; }
    }

    public class Alojamiento
    {
        public string Codigo { get; set; }
        public string Tipo { get; set; }
        public string Categoria { get; set; }
        public string Ubicacion { get; set; }
        public int Capacidad { get; set; }
        public decimal PrecioNoche { get; set; }
        public string Estado { get; set; }
        public string Caracteristicas { get; set; }
    }

    public class EmpleadoTemporal
    {
        public string NombreCompleto { get; set; }
        public string Tipo { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Idiomas { get; set; }
        public string Formacion { get; set; }
        public string Disponibilidad { get; set; }
    }

    public class Reserva
    {
        public string Codigo { get; set; }
        public string Alojamiento { get; set; }
        public string Cliente { get; set; }
        public DateTime Entrada { get; set; }
        public DateTime Salida { get; set; }
        public int Ocupantes { get; set; }
        public string Estado { get; set; }
        public string SolicitudesEspeciales { get; set; }
    }

    public class Actividad
    {
        public string Nombre { get; set; }
        public string Monitor { get; set; }
        public string Horario { get; set; }
        public string Destinatarios { get; set; }
        public int CupoMaximo { get; set; }
        public decimal PrecioHora { get; set; }
        public string Zona { get; set; }
    }
}
