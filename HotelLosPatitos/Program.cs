using Microsoft.EntityFrameworkCore;

namespace HotelLosPatitos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // esto es para obtener la cadena de conexión desde appsettings.json
            var connectionString = builder.Configuration.GetConnectionString("CadenaConexion");

            // esto para registrar el DbContext en el contenedor de dependencias
            builder.Services.AddDbContext<HotelLosPatitos.Infrastructure.Data.HotelDbContext>(options =>
                options.UseSqlServer(connectionString));

            // inyección de dependencias
            builder.Services.AddScoped<HotelLosPatitos.Domain.Interfaces.IHabitacionRepository, HotelLosPatitos.Infrastructure.Repositories.HabitacionRepository>();
            builder.Services.AddScoped<HotelLosPatitos.Domain.Interfaces.IReservacionRepository, HotelLosPatitos.Infrastructure.Repositories.ReservacionRepository>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
