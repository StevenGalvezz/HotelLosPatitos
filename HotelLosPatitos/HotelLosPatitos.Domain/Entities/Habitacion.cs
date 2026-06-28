using System;
using System.Collections.Generic;

namespace HotelLosPatitos.Domain.Entities
{
    public class Habitacion
    {
        public int Id { get; set; }
        public string CodigoDeHabitacion { get; set; } = null!;
        public string NombreDeHabitacion { get; set; } = null!;
        public int CantidadDeHuespedesPermitidos { get; set; }
        public int CantidadDeCamas { get; set; }
        public int CantidadDeBanos { get; set; }
        public string Ubicacion { get; set; } = null!;
        public string EncargadoDeLimpieza { get; set; } = null!;
        public int TipoDeHabitacion { get; set; } // 1- Junior, 2- Superior, 3- Suite
        public decimal CostoDeLimpieza { get; set; }
        public decimal CostoDeReserva { get; set; }
        public DateTime FechaDeRegistro { get; set; }
        public DateTime? FechaDeModificacion { get; set; }
        public bool Estado { get; set; } // true = Activo, false = Inactivo

        // Propiedad de navegación para la relación (Una habitación tiene muchas reservaciones)
        public virtual ICollection<Reservacion> Reservaciones { get; set; } = new List<Reservacion>();
    }
}
