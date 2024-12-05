using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using hippserver.Models.Entities;
using hippserver.Models.Domain;
using System.Reflection.Emit;
using hippserver.Models.JunctionTables;
using hippserver.Configurations;

namespace hippserver.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Order> Orders { get; set; }
        
        public DbSet<OrderHistory> orderHistories { get; set; }
        public DbSet<SalesPerson> salesPeople { get; set; }
        public DbSet<Product> Products { get; set; }
       
        public DbSet<Warehouse> Warehouse { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Apply Configurations from Configurations Folder
            builder.ApplyConfiguration(new ProductWarehouseConfiguration());
            builder.ApplyConfiguration(new OrderProductConfiguration());

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "ApplicationUsers");
            });

            builder.Entity<ApplicationRole>(entity =>
            {
                entity.ToTable(name: "ApplicationRoles");
            });

            
        }
    }
}