using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoomNest.Infrastructure;
using RoomNest.Services.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

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
                    options.EnableSensitiveDataLogging();//Disable in production
                    options.UseSqlServer(config.GetConnectionString("Dev"));
                });
            }
            else
            {
                services.AddDbContext<RoomNestDbContext>(options =>
                {
                    options.EnableSensitiveDataLogging();//Disable in production
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
                        
            //services.AddSingleton<IPropertyMappingService, PropertyMappingService>(); //It makes sense for singleton as even automapper is the same.
            //services.AddSingleton<IDataShaper<ReadArticle>, DataShaper<ReadArticle>>();
            services.AddTransient<IHotelService, HotelService>();
            services.AddTransient<IRoomService, RoomService>();
            services.AddTransient<IDBSeedService, DBSeedService>();
            services.AddTransient<IBookingService, BookingService>();
            //services.AddTransient<IArticleLikeService, ArticleLikeService>();
            //services.AddTransient<IDashboardService, DashboardService>();

            //services.AddAutoMapper(typeof(RoomNestMapper)); //Todo Used to work, Profile automatically discovered

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<RoomNestMapper>();
            });
        }

    }
}
