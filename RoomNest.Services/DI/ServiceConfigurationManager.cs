using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoomNest.Infrastructure.DBContext;
using RoomNest.Infrastructure.Interfaces;
using RoomNest.Infrastructure.Repos;
using RoomNest.Services.Implementations;
using RoomNest.Services.Interfaces;
using RoomNest.Services.Mapper;

namespace RoomNest.Services.DI
{
    public static class ServiceConfigurationManager
    {
        public static void ConfigurePersistence(IServiceCollection services, IConfiguration config, string envt)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");

            services.AddDbContext<RoomNestDbContext>(options =>
            {
                if (envt.Trim().ToUpper() == "DEVELOPMENT" ||
                    envt.Trim().ToUpper() == "CONTAINER")
                {
                    options.EnableSensitiveDataLogging();
                    options.EnableDetailedErrors(); // Useful in dev
                }
                options.EnableSensitiveDataLogging();// Valuable for debugging
                options.UseSqlServer(connectionString,
                                    sql => sql.MigrationsAssembly("RoomNest.Infrastructure"));
            });
        }

        public static void ConfigureServiceLifeTime(IServiceCollection services)
        {
            //We want to share the same DbContext instance throughout a single HTTP request.
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddScoped(typeof(IHotelRepository), typeof(HotelRepository));
            services.AddScoped(typeof(IRoomRepository), typeof(RoomRepository));
            services.AddScoped(typeof(IBookingRepository), typeof(BookingRepository));

            // Services - services depend on repositories (which are scoped).
            services.AddScoped<IHotelService, HotelService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IBookingService, BookingService>();

            //stateless utilities
            services.AddTransient<IDBSeedService, DBSeedService>();

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<RoomNestMapper>();
            });
        }
    }
}