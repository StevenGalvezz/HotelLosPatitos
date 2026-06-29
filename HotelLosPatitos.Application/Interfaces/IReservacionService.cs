using HotelLosPatitos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelLosPatitos.Application.Interfaces
{
    public interface IReservacionService
    {
        Task<IEnumerable<Reservacion>> ObtenerHistoricoPorHabitacionAsync(int idHabitacion);
        Task<Reservacion?> BuscarReservaPorIdAsync(int idReservacion);
        Task<Reservacion> CrearReservaAsync(Reservacion reservacion);
        decimal CalcularMontoTotal(DateTime fechaInicio, DateTime fechaFin, decimal costoReserva, decimal costoLimpieza);
    }
}
