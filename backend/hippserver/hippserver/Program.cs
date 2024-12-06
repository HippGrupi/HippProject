using DotNetEnv;
using Microsoft.Data.SqlClient;
using System.Data;
using hippserver.Helpers;
using hippserver.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using hippserver.Models.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using hippserver.Configurations;
using hippserver.Services.Interfaces;
using hippserver.Services.Implementations;
using hippserver.Infrastructure.Repositories.Implementations;
using hippserver.Infrastructure.Repositories.Interfaces;

namespace hippserver
{
    public class Program
    {
        //=================
        //MAIN METHOD
        //=================

        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

           
            Env.Load();



           
            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();

           
            await SeedDataAsync(app);


            // Configure middleware
            ConfigureMiddleware(app);

            app.Run();
        }


        //===================
        //DATA SEEDER
        //===================
        private static async Task SeedDataAsync(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogInformation("Starting data seeding process...");

                var seeder = services.GetRequiredService<DataSeeder>();
                await seeder.SeedAsync();

                logger.LogInformation("Data seeding completed.");
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while seeding the database");
            }
        }

        //==================
        //Configure Services
        //==================

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            



            // Controllers
            services.AddControllers()
                    .AddJsonOptions(options =>
                        {
                            options.JsonSerializerOptions.ReferenceHandler =                System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;

                            options.JsonSerializerOptions.PropertyNamingPolicy = null;
                         });

            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "HIPP API",
                    Version = "v1"
                });

                // Configure Swagger to use JWT
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
            

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

           
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            
            
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtService, JwtService>();

           
            services.AddScoped<IDbConnection>(sp =>
                new SqlConnection(configuration.GetConnectionString("DefaultConnection")));


            // Identity
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = true;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // JWT Settings
            var jwtSettings = new JwtSettings();

            // JWT Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Secret))
                };
            });

           
            

            // Register DataSeeder
            services.AddScoped<DataSeeder>();

            // CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });
        }



        //====================
        //Configure Middleware
        //====================


        private static void ConfigureMiddleware(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "HIPP API V1");
                });
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowSpecificOrigins");

            // Authentication & Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
        }
    }
}