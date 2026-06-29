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
            // Se traen los datos de la habitación para usar sus costos reales guardados en la BD
            var habitacion = await _habitacionRepository.ObtenerPorIdAsync(reservacion.IdHabitacion);
            if (habitacion == null)
                throw new Exception("La habitación seleccionada no existe.");

            // Inactiva = no se puede reservar
            if (!habitacion.Estado)
                throw new Exception("Estimado usuario, la habitación seleccionada se encuentra inactiva.");

            // Validar que no existan choques de fechas con otras reservas existentes
            bool estaOcupada = await _reservacionRepository.ValidarDisponibilidadAsync(reservacion.IdHabitacion, reservacion.FechaInicioReserva, reservacion.FechaFinReserva);

            if (estaOcupada)
                throw new Exception("Estimado usuario, la habitación seleccionada ya se encuentra reservada para las fechas seleccionadas.");

            // Cálculo del monto total automáticamente con la fórmula
            reservacion.MontoTotal = CalcularMontoTotal(
                reservacion.FechaInicioReserva,
                reservacion.FechaFinReserva,
                habitacion.CostoDeReserva,
                habitacion.CostoDeLimpieza
            );

            // Estampamos la fecha y hora exacta del registro
            reservacion.FechaDeRegistro = DateTime.Now;

            // CORRECCIÓN: Ejecutamos el método directamente sin guardarlo en una variable 'void'
            await _reservacionRepository.AgregarAsync(reservacion);

            // Devolvemos el mismo objeto 'reservacion', el cual ya lleva el ID asignado automáticamente por EF
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
