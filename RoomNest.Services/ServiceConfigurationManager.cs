using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoomNest.Infrastructure;
using RoomNest.Services.Mapper;

namespace RoomNest.Services
{
    public static class ServiceConfigurationManager
    {
        public static void ConfigurePersistence(IServiceCollection services, IConfiguration config, string envt)
        {
            if (envt.Trim().ToUpper() == "DEVELOPMENT")
            {
                services.AddDbContext<RoomNestDbContext>(options =>
                {
                    options.EnableSensitiveDataLogging();// Valuable for debugging
                    options.UseSqlServer(config.GetConnectionString("Dev"));
                });
            }
            else
            {
                services.AddDbContext<RoomNestDbContext>(options =>
                {
                   //options.EnableSensitiveDataLogging();//Disabled in production
                    options.UseSqlServer(config.GetConnectionString("Prod"),
                            b => b.MigrationsAssembly("RoomNest.Infrastructure"));
                });
            }
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