using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RoomNest.API.Middleware;
using RoomNest.API.OperationFilter;
using RoomNest.Infrastructure.DBContext;
using RoomNest.Services.DI;
using System.Reflection;

namespace RoomNest.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Security Vulnerability not fit for real-life production
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            builder.Services.AddControllers()
                             .AddJsonOptions(opt =>
                             {
                                 opt.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
                             });

            ServiceConfigurationManager.ConfigurePersistence(builder.Services, builder.Configuration, builder.Environment.EnvironmentName);
            ServiceConfigurationManager.ConfigureServiceLifeTime(builder.Services);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                // add a custom operation filter which sets default values
                c.OperationFilter<SwaggerDefaultValuesFilter>();
                c.OperationFilter<RequestExamplesFilter>();

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "RoomNest Hotel Booking API",
                    Version = "v1.0.0",
                    Description = "Comprehensive hotel room booking and availability management API",
                    Contact = new OpenApiContact
                    {
                        Name = "RoomNest Support Team",
                        Email = "support@roomnest.com",
                        Url = new Uri("https://roomnest.com/support")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    },
                    TermsOfService = new Uri("https://roomnest.com/terms")
                });

                // XML Comments for automatic documentation
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            });

            var app = builder.Build();

            await ApplyDBMigration(app);

            // Register global exception handling middleware BEFORE other middleware
            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

            app.UseCors("default");
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }

        private static async Task ApplyDBMigration(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<RoomNestDbContext>();
                var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                if (pendingMigrations.Any())
                {
                    logger.LogInformation($"Applying {pendingMigrations.Count()} pending migrations...");
                    await dbContext.Database.MigrateAsync();
                    logger.LogInformation("Migrations applied successfully.");
                }
                else
                {
                    logger.LogInformation("Database is up to date.");
                }
            }
        }
    }
}