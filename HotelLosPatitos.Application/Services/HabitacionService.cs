using HotelLosPatitos.Application.Interfaces;
using HotelLosPatitos.Domain.Entities;
using HotelLosPatitos.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelLosPatitos.Application.Services
{
    public class HabitacionService : IHabitacionService
    {
        private readonly IHabitacionRepository _habitacionRepository;

        // Inyectamos el repositorio por constructor (Principio SOLID de Inversión de Dependencias)
        public HabitacionService(IHabitacionRepository habitacionRepository)
        {
            _habitacionRepository = habitacionRepository;
        }

        public async Task<IEnumerable<Habitacion>> ObtenerTodasLasHabitacionesAsync()
        {
            return await _habitacionRepository.ObtenerTodasAsync();
        }

        public async Task<IEnumerable<Habitacion>> ObtenerHabitacionesDisponiblesAsync()
        {
            return await _habitacionRepository.ObtenerDisponiblesAsync();
        }

        public async Task<Habitacion?> ObtenerHabitacionPorIdAsync(int id)
        {
            return await _habitacionRepository.ObtenerPorIdAsync(id);
        }

        public async Task RegistrarHabitacionAsync(Habitacion habitacion)
        {
            // validaciones
            if (habitacion.CantidadDeHuespedesPermitidos <= 0)
                throw new ArgumentException("La cantidad de huéspedes permitidos debe ser mayor a 0.");

            if (habitacion.CostoDeLimpieza <= 0 || habitacion.CostoDeReserva <= 0)
                throw new ArgumentException("Los costos de limpieza y reserva deben ser mayores a 0.");

            // campos automáticos (no se les pide al usuario)
            habitacion.FechaDeRegistro = DateTime.Now;
            habitacion.FechaDeModificacion = null;

            await _habitacionRepository.AgregarAsync(habitacion);
        }

        public async Task EditarHabitacionAsync(Habitacion habitacionExistente)
        {
            // "cuando se edita se debe de modificar la fecha de modificación, pero no se le solicita al usuario"
            habitacionExistente.FechaDeModificacion = DateTime.Now;

            if (habitacionExistente.CantidadDeHuespedesPermitidos <= 0)
                throw new ArgumentException("La cantidad de huéspedes permitidos debe ser mayor a 0.");

            if (habitacionExistente.CostoDeLimpieza <= 0 || habitacionExistente.CostoDeReserva <= 0)
                throw new ArgumentException("Los costos operativos deben ser mayores a 0.");

            await _habitacionRepository.ActualizarAsync(habitacionExistente);
        }
    }
}
