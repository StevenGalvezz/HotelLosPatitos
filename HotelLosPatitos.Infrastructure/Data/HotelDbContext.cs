using HotelLosPatitos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelLosPatitos.Infrastructure.Data
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options)
        {
        }

        // mapear las entidades a las tablas de la base de datos
        public DbSet<Habitacion> Habitaciones { get; set; } = null!;
        public DbSet<Reservacion> Reservaciones { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // configuración explícita tabla HABITACIONES
            modelBuilder.Entity<Habitacion>(entity =>
            {
                entity.ToTable("HABITACIONES");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CodigoDeHabitacion).HasMaxLength(7).IsRequired();
                entity.Property(e => e.NombreDeHabitacion).HasMaxLength(30).IsRequired();
                entity.Property(e => e.Ubicacion).HasMaxLength(10).IsRequired();
                entity.Property(e => e.EncargadoDeLimpieza).HasMaxLength(100).IsRequired();
                entity.Property(e => e.CostoDeLimpieza).HasColumnType("decimal(18,2)");
                entity.Property(e => e.CostoDeReserva).HasColumnType("decimal(18,2)");
            });

            // configuración explícita tabla RESERVACIONES
            modelBuilder.Entity<Reservacion>(entity =>
            {
                entity.ToTable("RESERVACIONES");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NombreDeLaPersona).HasMaxLength(150).IsRequired();
                entity.Property(e => e.Identificacion).HasMaxLength(30).IsRequired();
                entity.Property(e => e.Telefono).HasMaxLength(10).IsRequired();
                entity.Property(e => e.Correo).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Direccion).HasMaxLength(200).IsRequired();
                entity.Property(e => e.MontoTotal).HasColumnType("decimal(18,2)");

                // Configuración de la llave foránea e inversión de la relación
                entity.HasOne(d => d.Habitacion)
                    .WithMany(p => p.Reservaciones)
                    .HasForeignKey(d => d.IdHabitacion)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
        }
    }
}
