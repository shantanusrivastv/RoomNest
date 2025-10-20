
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using RoomNest.API.Middleware;
using RoomNest.Model;
using RoomNest.Services;
using System.Reflection;
using System.Text.Json.Serialization;

namespace RoomNest.API
{
    public class Program
    {
        public static void Main(string[] args)
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


            // Register global exception handling middleware BEFORE other middleware
            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
