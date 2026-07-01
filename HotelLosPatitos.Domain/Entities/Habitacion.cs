using System;
using System.ComponentModel.DataAnnotations;

namespace HotelLosPatitos.Domain.Entities
{
    public class Habitacion
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El código de la habitación es obligatorio.")]
        [StringLength(7, ErrorMessage = "El código no puede superar los 7 caracteres.")]
        [Display(Name = "Código")]
        public string CodigoDeHabitacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre de la habitación es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
        [Display(Name = "Nombre de la Habitación")]
        public string NombreDeHabitacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La ubicación es obligatoria.")]
        [StringLength(10, ErrorMessage = "La ubicación no puede superar los 10 caracteres.")]
        [Display(Name = "Ubicación")]
        public string Ubicacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La cantidad de huéspedes es obligatoria.")]
        [Range(1, 20, ErrorMessage = "La cantidad de huéspedes permitidos debe estar entre 1 y 20.")]
        [Display(Name = "Máx. Huéspedes")]
        public int CantidadDeHuespedesPermitidos { get; set; }

        [Required(ErrorMessage = "La cantidad de camas es obligatoria.")]
        [Range(0, 10, ErrorMessage = "La cantidad de camas debe estar entre 0 y 10.")]
        [Display(Name = "Cantidad de Camas")]
        public int CantidadDeCamas { get; set; }

        [Required(ErrorMessage = "La cantidad de baños es obligatoria.")]
        [Range(0, 5, ErrorMessage = "La cantidad de baños debe estar entre 0 y 5.")]
        [Display(Name = "Cantidad de Baños")]
        public int CantidadDeBanos { get; set; }

        [Required(ErrorMessage = "El encargado de limpieza es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre del encargado no puede superar los 100 caracteres.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El encargado de limpieza solo debe contener letras y espacios.")]
        [Display(Name = "Encargado de Limpieza")]
        public string EncargadoDeLimpieza { get; set; } = string.Empty;

        [Required(ErrorMessage = "El costo de limpieza es obligatorio.")]
        [Range(1.00, 500000.00, ErrorMessage = "El costo de limpieza debe ser un monto lógico entre ₡1.00 y ₡500,000.00.")]
        [Display(Name = "Costo de Limpieza")]
        public decimal CostoDeLimpieza { get; set; }

        [Required(ErrorMessage = "El costo de reserva es obligatorio.")]
        [Range(1.00, 2000000.00, ErrorMessage = "El costo de reserva debe estar entre ₡1.00 y ₡2,000,000.00 por noche.")]
        [Display(Name = "Costo de Reserva por Día")]
        public decimal CostoDeReserva { get; set; }

        [Required]
        public int TipoDeHabitacion { get; set; }

        [Required]
        public bool Estado { get; set; }

        public DateTime FechaDeRegistro { get; set; }

        public DateTime? FechaDeModificacion { get; set; }

        // propiedad de navegación para la relación (Una habitación tiene muchas reservaciones)
        public virtual ICollection<Reservacion> Reservaciones { get; set; } = new List<Reservacion>();
    }
}
