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
            // Buscamos la habitación de nuevo para recargar sus costos operativos
            var habitacion = await _habitacionService.ObtenerHabitacionPorIdAsync(reservacion.IdHabitacion);

            if (habitacion == null)
            {
                return NotFound();
            }

            // Validaciones básicas de fechas antes de mandar al servicio
            if (reservacion.FechaFinReserva <= reservacion.FechaInicioReserva)
            {
                ModelState.AddModelError("FechaFinReserva", "La fecha de fin debe ser posterior a la fecha de inicio.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // El servicio se encarga de calcular el MontoTotal automáticamente con la fórmula
                    var resultado = await _reservacionService.CrearReservaAsync(reservacion);

                    // Si todo se guarda con éxito, mandamos al usuario directo a la vista de detalles (Factura)
                    return RedirectToAction(nameof(Detalles), new { idReservacion = resultado.Id });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            // Si hubo un error, volvemos a pasar la habitación para no romper la vista
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

            // Traemos la habitación vinculada para poder mostrar los costos unitarios en la factura
            var habitacion = await _habitacionService.ObtenerHabitacionPorIdAsync(reservacion.IdHabitacion);
            ViewData["Habitacion"] = habitacion;

            return View(reservacion);
        }

    }
}
