using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using hippserver.Configurations;
using hippserver.Infrastructure.Data;
using hippserver.Infrastructure.Repositories.Implementations;
using hippserver.Infrastructure.Repositories.Interfaces;
using hippserver.Models.Entities;
using hippserver.Services.Implementations;
using hippserver.Services.Interfaces;
using hippserver.Models.Domain;
using System.Security.Claims;

namespace hippserver
{
    public class Program
    {
       

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            

            // Add services to the container
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
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

            // Register Repositories
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IBaseRepository<Employee>, BaseRepository<Employee>>();
            builder.Services.AddScoped<IBaseRepository<Product>, BaseRepository<Product>>();
            builder.Services.AddScoped<IBaseRepository<Order>, BaseRepository<Order>>();
            builder.Services.AddScoped<IBaseRepository<Driver>, BaseRepository<Driver>>();
            builder.Services.AddScoped<IBaseRepository<SalesPerson>, BaseRepository<SalesPerson>>();

            builder.Services.AddScoped<DataSeeder>();

            // Register Services
            builder.Services.AddScoped<IManagerService, ManagerService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IJwtService, JwtService>();


            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
              {
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = false,
                      ValidateAudience = false,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,
                      ValidIssuer = builder.Configuration["Jwt:Issuer"],
                      ValidAudience = builder.Configuration["Jwt:Audience"],
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                  };
                  options.Events = new JwtBearerEvents
                  {
                      OnTokenValidated = context =>
                      {
                          var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                          var userClaims = claimsIdentity.Claims;
                          // Log claims for debugging
                          Console.WriteLine("Token validated successfully.");
                          return Task.CompletedTask;
                      },
                      OnAuthenticationFailed = context =>
                      {
                          // Log the exception
                          Console.WriteLine("Authentication failed: " + context.Exception.Message);
                          return Task.CompletedTask;
                      }
                  };
              });




            builder.Services.AddControllers();

           builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Description = "Bearer Authentication with JWT Token",
                    Type = SecuritySchemeType.Http
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });
            });

           

            // Build the app
            var app = builder.Build();

            // Middleware configuration
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors(options =>
            {
                options.AllowAnyHeader();
                options.AllowAnyOrigin();
                options.AllowAnyMethod();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            // Database seeding
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dataSeeder = services.GetRequiredService<DataSeeder>();
                dataSeeder.SeedAsync().GetAwaiter().GetResult();
            }

            app.Run();
        }
    }
}
