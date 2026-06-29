using HotelLosPatitos.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelLosPatitos.Domain.Interfaces
{
    public interface IReservacionRepository
    {
        Task<IEnumerable<Reservacion>> ObtenerHistoricoPorHabitacionAsync(int idHabitacion);
        Task<Reservacion?> ObtenerPorIdAsync(int id);
        Task AgregarAsync(Reservacion reservacion);
        Task<bool> ValidarDisponibilidadAsync(int idHabitacion, DateTime inicio, DateTime fin);
    }
}
