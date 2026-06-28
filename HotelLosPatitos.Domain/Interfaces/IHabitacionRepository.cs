using HotelLosPatitos.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelLosPatitos.Domain.Interfaces
{
    public interface IHabitacionRepository
    {
        Task<IEnumerable<Habitacion>> ObtenerTodasAsync();
        Task<IEnumerable<Habitacion>> ObtenerDisponiblesAsync();
        Task<Habitacion?> ObtenerPorIdAsync(int id);
        Task AgregarAsync(Habitacion habitacion);
        Task ActualizarAsync(Habitacion habitacion);
    }
}
