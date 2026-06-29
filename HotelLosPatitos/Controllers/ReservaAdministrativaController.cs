using HotelLosPatitos.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HotelLosPatitos.Web.Controllers
{
    public class ReservaAdministrativaController : Controller
    {
        private readonly IReservacionService _reservacionService;
        private readonly IHabitacionService _habitacionService;

        // inyectra los servicios necesarios
        public ReservaAdministrativaController(IReservacionService reservacionService, IHabitacionService habitacionService)
        {
            _reservacionService = reservacionService;
            _habitacionService = habitacionService;
        }

        // HISTORIAL DE RESERVAS (Recibe un filtro opcional por ID de Habitación)
        // URL: /ReservaAdministrativa?idHabitacion=5
        public async Task<IActionResult> Index(int? idHabitacion)
        {
            if (idHabitacion == null)
            {
                // entrar sin filtro = mandar un listado vacío
                return RedirectToAction("Index", "Habitaciones");
            }

            // se traen los datos de la habitación para mostrar el nombre en el encabezado
            var habitacion = await _habitacionService.ObtenerHabitacionPorIdAsync(idHabitacion.Value);
            if (habitacion == null)
            {
                return NotFound();
            }

            // Guardamos el nombre en el ViewData para usarlo en el diseño de la vista
            ViewData["NombreHabitacion"] = habitacion.NombreDeHabitacion;
            ViewData["CodigoHabitacion"] = habitacion.CodigoDeHabitacion;

            // Consultamos las reservas asociadas a esta habitación específica
            var reservas = await _reservacionService.ObtenerHistoricoPorHabitacionAsync(idHabitacion.Value);

            return View(reservas);
        }
    }
}
