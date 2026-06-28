using HotelLosPatitos.Domain.Entities;
using HotelLosPatitos.Domain.Interfaces;
using HotelLosPatitos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelLosPatitos.Infrastructure.Repositories
{
    public class HabitacionRepository : IHabitacionRepository
    {
        private readonly HotelDbContext _context;

        public HabitacionRepository(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Habitacion>> ObtenerTodasAsync()
        {
            // devuelve todas las habitaciones registradas para el módulo de administración
            return await _context.Habitaciones.ToListAsync();
        }

        public async Task<IEnumerable<Habitacion>> ObtenerDisponiblesAsync()
        {
            // "si una habitación se inactiva, no se podrá utilizar para reservar"
            // filtrar solo las que tengan Estado == true (Activo)
            return await _context.Habitaciones
                                 .Where(h => h.Estado == true)
                                 .ToListAsync();
        }

        public async Task<Habitacion?> ObtenerPorIdAsync(int id)
        {
            return await _context.Habitaciones.FindAsync(id);
        }

        public async Task AgregarAsync(Habitacion habitacion)
        {
            await _context.Habitaciones.AddAsync(habitacion);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Habitacion habitacion)
        {
            _context.Habitaciones.Update(habitacion);
            await _context.SaveChangesAsync();
        }
    }
}
