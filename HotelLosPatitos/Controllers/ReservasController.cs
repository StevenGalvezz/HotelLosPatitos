using HotelLosPatitos.Application.Interfaces;
using HotelLosPatitos.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HotelLosPatitos.Web.Controllers
{
    public class ReservasController : Controller
    {
        private readonly IReservacionService _reservacionService;
        private readonly IHabitacionService _habitacionService;

        public ReservasController(IReservacionService reservacionService, IHabitacionService habitacionService)
        {
            _reservacionService = reservacionService;
            _habitacionService = habitacionService;
        }

        // listar habitaciones disponibles
        // URL: /Reservas
        public async Task<IActionResult> Index()
        {
            // esta será una vista donde inicialmente se muestran las habitaciones que el hotel tiene disponibles
            var habitacionesDisponibles = await _habitacionService.ObtenerHabitacionesDisponiblesAsync();
            return View(habitacionesDisponibles);
        }

        // formulario de reservación (Precarga los datos de la habitación)
        // GET: /Reservas/Reservar?idHabitacion=5
        public async Task<IActionResult> Reservar(int? idHabitacion)
        {
            if (idHabitacion == null)
            {
                return RedirectToAction(nameof(Index));
            }

            // se busca la habitación para verificar que exista y esté activa
            var habitacion = await _habitacionService.ObtenerHabitacionPorIdAsync(idHabitacion.Value);
            if (habitacion == null || !habitacion.Estado)
            {
                TempData["MensajeError"] = "La habitación seleccionada no está disponible.";
                return RedirectToAction(nameof(Index));
            }

            // Pasamos la habitación por el ViewData para mostrar sus datos y costos en la factura visual
            ViewData["Habitacion"] = habitacion;

            // Creamos un objeto de reservación vacío y le preasignamos el IdHabitacion
            var nuevaReservacion = new Reservacion
            {
                IdHabitacion = habitacion.Id,
                FechaInicioReserva = DateTime.Now.Date,
                FechaFinReserva = DateTime.Now.Date.AddDays(1) // Por defecto sugerimos 1 noche
            };

            return View(nuevaReservacion);
        }

        // procesar reservacion (Cálculo automático y guardado)
        // POST: /Reservas/Reservar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reservar(Reservacion reservacion)
        {
            var habitacion = await _habitacionService.ObtenerHabitacionPorIdAsync(reservacion.IdHabitacion);
            if (habitacion == null)
            {
                return NotFound();
            }

            // 1. VALIDACIÓN: Evitar que reserven en días pasados
            if (reservacion.FechaInicioReserva.Date < DateTime.Today)
            {
                ModelState.AddModelError("FechaInicioReserva", "No se permiten reservaciones para fechas anteriores al día de hoy.");
            }

            // 2. VALIDACIÓN: Las fechas de fin deben ser coherentes
            if (reservacion.FechaFinReserva.Date <= reservacion.FechaInicioReserva.Date)
            {
                ModelState.AddModelError("FechaFinReserva", "La fecha de Check-Out (Fin) debe ser posterior a la fecha de Check-In (Inicio).");
            }

            // 3. VALIDACIÓN: Mayoría de edad
            var edad = DateTime.Today.Year - reservacion.FechaNacimiento.Year;
            if (reservacion.FechaNacimiento.Date > DateTime.Today.AddYears(-edad)) edad--;
            if (edad < 18)
            {
                ModelState.AddModelError("FechaNacimiento", "Estimado usuario, debe ser mayor de edad (18 años o más) para registrar una reservación.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // El servicio se encargará de validar el cruce de fechas internamente antes de guardar
                    var resultado = await _reservacionService.CrearReservaAsync(reservacion);
                    return RedirectToAction(nameof(Detalles), new { idReservacion = resultado.Id });
                }
                catch (Exception ex)
                {
                    // se captura el mensaje que tire el servicio (por ejemplo, si la habitación está ocupada)
                    var mensajeDetallado = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    ModelState.AddModelError("", mensajeDetallado);
                }
            }

            ViewData["Habitacion"] = habitacion;
            return View(reservacion);
        }

        // buscar reserva por ID (Acción que usa el buscador de la pantalla principal)
        // GET: /Reservas/Buscar?idReservacion=5
        public async Task<IActionResult> Buscar(int idReservacion)
        {
            var reservacion = await _reservacionService.BuscarReservaPorIdAsync(idReservacion);

            if (reservacion == null)
            {
                // REQUISITO: Mensaje exacto exigido en el enunciado si no existe
                TempData["MensajeError"] = "Estimado usuario, no se ha encontrado la reservación, favor realice una";
                return RedirectToAction(nameof(Index));
            }

            // Si la encuentra, lo redirige a la vista de detalles
            return RedirectToAction(nameof(Detalles), new { idReservacion = reservacion.Id });
        }

        // vista detalle / factura
        // GET: /Reservas/Detalles?idReservacion=5
        public async Task<IActionResult> Detalles(int? idReservacion)
        {
            if (idReservacion == null)
            {
                return NotFound();
            }

            var reservacion = await _reservacionService.BuscarReservaPorIdAsync(idReservacion.Value);
            if (reservacion == null)
            {
                return NotFound();
            }

            // se trae la habitación vinculada para poder mostrar los costos unitarios en la factura
            var habitacion = await _habitacionService.ObtenerHabitacionPorIdAsync(reservacion.IdHabitacion);
            ViewData["Habitacion"] = habitacion;

            return View(reservacion);
        }

    }
}
