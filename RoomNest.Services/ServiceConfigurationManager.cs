using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoomNest.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            ////We want to share the same DbContext instance throughout a single HTTP request.
            //services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            //services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            ////The service layer is stateless hence transient
            ////services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            //services.AddSingleton<IPropertyMappingService, PropertyMappingService>(); //It makes sense for singleton as even automapper is the same.
            //services.AddSingleton<IDataShaper<ReadArticle>, DataShaper<ReadArticle>>();
            //services.AddTransient<IArticleServices, ArticleServices>();
            //services.AddTransient<IUserService, UserService>();
            //services.AddTransient<IArticleLikeService, ArticleLikeService>();
            //services.AddTransient<IDashboardService, DashboardService>();
            //services.AddAutoMapper(typeof(Mapper));
        }

    }
}
