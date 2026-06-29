using System;
using System.ComponentModel.DataAnnotations; // Asegúrate de tener este namespace arriba

namespace HotelLosPatitos.Domain.Entities
{
    public class Reservacion
    {
        public int Id { get; set; }

        public int IdHabitacion { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El nombre solo debe contener letras y espacios.")]
        [StringLength(150)]
        [Display(Name = "Nombre Completo")]
        public string NombreDeLaPersona { get; set; } = string.Empty;

        [Required(ErrorMessage = "La identificación es obligatoria.")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "La identificación solo permite letras y números sin guiones ni espacios.")]
        [StringLength(30)]
        [Display(Name = "Identificación")]
        public string Identificacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [RegularExpression(@"^[0-9]{8,10}$", ErrorMessage = "El teléfono debe contener entre 8 y 10 dígitos numéricos.")]
        [StringLength(10)]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Por favor, ingrese un correo electrónico válido.")]
        [StringLength(50)]
        [Display(Name = "Correo Electrónico")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(200)]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Fecha de nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        public DateTime FechaInicioReserva { get; set; }

        [Required]
        public DateTime FechaFinReserva { get; set; }

        public decimal MontoTotal { get; set; }

        public DateTime FechaDeRegistro { get; set; }

        public Habitacion? Habitacion { get; set; }
    }
}
