using HotelLosPatitos.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelLosPatitos.Application.Interfaces
{
    public interface IHabitacionService
    {
        Task<IEnumerable<Habitacion>> ObtenerTodasLasHabitacionesAsync();
        Task<IEnumerable<Habitacion>> ObtenerHabitacionesDisponiblesAsync();
        Task<Habitacion?> ObtenerHabitacionPorIdAsync(int id);
        Task RegistrarHabitacionAsync(Habitacion habitacion);
        Task EditarHabitacionAsync(Habitacion habitacion);
    }
}
