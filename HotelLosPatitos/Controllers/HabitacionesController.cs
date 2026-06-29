using HotelLosPatitos.Application.Interfaces;
using HotelLosPatitos.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HotelLosPatitos.Web.Controllers
{
    public class HabitacionesController : Controller
    {
        private readonly IHabitacionService _habitacionService;

        // El contenedor de dependencias nos pasa automáticamente el servicio aquí
        public HabitacionesController(IHabitacionService habitacionService)
        {
            _habitacionService = habitacionService;
        }

        // 1. LISTAR HABITACIONES (Vista Principal del Administrador)
        // URL: /Habitaciones
        public async Task<IActionResult> Index()
        {
            var habitaciones = await _habitacionService.ObtenerTodasLasHabitacionesAsync();
            return View(habitaciones);
        }

        // 2. REGISTRAR HABITACIÓN (Mostrar Formulario)
        // GET: /Habitaciones/Create
        public IActionResult Create()
        {
            return View();
        }

        // 3. REGISTRAR HABITACIÓN (Procesar y Guardar Datos)
        // POST: /Habitaciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Habitacion habitacion)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // se fuerza el estado inicial en activo al registrar (1 - Activo)
                    habitacion.Estado = true;

                    // llamaar al servicio que contiene las validaciones de negocio
                    await _habitacionService.RegistrarHabitacionAsync(habitacion);

                    // todo sale bien = se redirige a la tabla principal
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    // el servicio detecta un error de negocio = se captura y se muestra en la pantalla
                    ModelState.AddModelError("", ex.Message);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ocurrió un error inesperado al registrar la habitación.");
                }
            }

            // si el modelo no es válido o hubo un error = se vuelve a mostrar el formulario con los datos que ya puso
            return View(habitacion);
        }

        // 4. EDIDAR HABITACIÓN (Mostrar Formulario con Datos Existentes)
        // GET: /Habitaciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habitacion = await _habitacionService.ObtenerHabitacionPorIdAsync(id.Value);
            if (habitacion == null)
            {
                return NotFound();
            }

            return View(habitacion);
        }

        // 5. EDITAR HABITACIÓN (Procesar Cambios)
        // POST: /Habitaciones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Habitacion habitacionModificada)
        {
            if (id != habitacionModificada.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // se busca el registro original en la BD para no perder los campos que no se editan
                    var habitacionExistente = await _habitacionService.ObtenerHabitacionPorIdAsync(id);
                    if (habitacionExistente == null)
                    {
                        return NotFound();
                    }

                    // mapear únicamente los campos permitidos por el enunciado
                    habitacionExistente.CantidadDeHuespedesPermitidos = habitacionModificada.CantidadDeHuespedesPermitidos;
                    habitacionExistente.CantidadDeCamas = habitacionModificada.CantidadDeCamas;
                    habitacionExistente.EncargadoDeLimpieza = habitacionModificada.EncargadoDeLimpieza;
                    habitacionExistente.TipoDeHabitacion = habitacionModificada.TipoDeHabitacion;
                    habitacionExistente.CostoDeLimpieza = habitacionModificada.CostoDeLimpieza;
                    habitacionExistente.CostoDeReserva = habitacionModificada.CostoDeReserva;
                    habitacionExistente.Estado = habitacionModificada.Estado;

                    // El servicio se encargará de estampar la FechaDeModificacion automáticamente
                    await _habitacionService.EditarHabitacionAsync(habitacionExistente);

                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ocurrió un error inesperado al actualizar la habitación.");
                }
            }

            return View(habitacionModificada);
        }

    }
}
