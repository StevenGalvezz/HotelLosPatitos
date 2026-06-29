using HotelLosPatitos.Domain.Entities;
using HotelLosPatitos.Domain.Interfaces;
using HotelLosPatitos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelLosPatitos.Infrastructure.Repositories
{
    public class ReservacionRepository : IReservacionRepository
    {
        private readonly HotelDbContext _context;

        public ReservacionRepository(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reservacion>> ObtenerHistoricoPorHabitacionAsync(int idHabitacion)
        {
            // Módulo administrativo: Filtra la lista de reservas realizadas en específico por el id de habitación
            return await _context.Reservaciones
                                 .Where(r => r.IdHabitacion == idHabitacion)
                                 .ToListAsync();
        }

        public async Task<Reservacion?> ObtenerPorIdAsync(int id)
        {
            // módulo "Buscar Reserva", se trae también los datos de la habitación asociada
            return await _context.Reservaciones
                                 .Include(r => r.Habitacion)
                                 .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task AgregarAsync(Reservacion reservacion)
        {
            await _context.Reservaciones.AddAsync(reservacion);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ValidarDisponibilidadAsync(int idHabitacion, DateTime inicio, DateTime fin)
        {
            // Regla matemática de cruce de rangos: (Inicio1 <= Fin2) Y (Fin1 >= Inicio2)
            return await _context.Reservaciones.AnyAsync(r =>
                r.IdHabitacion == idHabitacion &&
                r.FechaInicioReserva.Date < fin.Date &&
                r.FechaFinReserva.Date > inicio.Date);
        }
    }
}
