using HotelLosPatitos.Application.Interfaces;
using HotelLosPatitos.Domain.Entities;
using HotelLosPatitos.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelLosPatitos.Application.Services
{
    public class ReservacionService : IReservacionService
    {
        private readonly IReservacionRepository _reservacionRepository;
        private readonly IHabitacionRepository _habitacionRepository;

        public ReservacionService(IReservacionRepository reservacionRepository, IHabitacionRepository habitacionRepository)
        {
            _reservacionRepository = reservacionRepository;
            _habitacionRepository = habitacionRepository;
        }

        public async Task<IEnumerable<Reservacion>> ObtenerHistoricoPorHabitacionAsync(int idHabitacion)
        {
            return await _reservacionRepository.ObtenerHistoricoPorHabitacionAsync(idHabitacion);
        }

        public async Task<Reservacion?> BuscarReservaPorIdAsync(int idReservacion)
        {
            return await _reservacionRepository.ObtenerPorIdAsync(idReservacion);
        }

        public async Task<Reservacion> CrearReservaAsync(Reservacion reservacion)
        {
            // se traen los datos de la habitación para usar sus costos reales guardados en la BD
            var habitacion = await _habitacionRepository.ObtenerPorIdAsync(reservacion.IdHabitacion);
            if (habitacion == null)
                throw new Exception("La habitación seleccionada no existe.");

            // inactiva = no se puede reservar
            if (!habitacion.Estado)
                throw new Exception("Estimado usuario, la habitación seleccionada se encuentra inactiva.");

            // calculo del monto total automaticamente con la fórmula
            reservacion.MontoTotal = CalcularMontoTotal(
                reservacion.FechaInicioReserva,
                reservacion.FechaFinReserva,
                habitacion.CostoDeReserva,
                habitacion.CostoDeLimpieza
            );

            reservacion.FechaDeRegistro = DateTime.Now;

            await _reservacionRepository.AgregarAsync(reservacion);
            return reservacion;
        }

        public decimal CalcularMontoTotal(DateTime fechaInicio, DateTime fechaFin, decimal costoReserva, decimal costoLimpieza)
        {
            // calculo de los días exactos de diferencia
            int dias = (fechaFin.Date - fechaInicio.Date).Days;

            // Validación lógica: si entran y salen el mismo día, se cuenta como 1 noche mínimo
            if (dias <= 0) dias = 1;

            // Fórmula: (cantidadDiasReserva * CostoDeReserva) + CostoDeLimpieza
            return (dias * costoReserva) + costoLimpieza;
        }
    }
}
