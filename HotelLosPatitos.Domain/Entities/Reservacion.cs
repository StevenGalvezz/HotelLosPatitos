using System;

namespace HotelLosPatitos.Domain.Entities
{
    public class Reservacion
    {
        public int Id { get; set; }
        public string NombreDeLaPersona { get; set; } = null!;
        public string Identificacion { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public DateTime FechaNacimiento { get; set; }
        public string Direccion { get; set; } = null!;
        public decimal MontoTotal { get; set; }
        public DateTime FechaInicioReserva { get; set; }
        public DateTime FechaFinReserva { get; set; }
        public DateTime FechaDeRegistro { get; set; }
        public int IdHabitacion { get; set; }

        // propiedad de navegación (La reservación pertenece a una habitación)
        public virtual Habitacion Habitacion { get; set; } = null!;
    }
}
