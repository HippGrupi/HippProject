using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using hippserver.Models.JunctionTables;
using System.Reflection.Emit;

namespace hippserver.Configurations
{
    public class ProductWarehouseConfiguration : IEntityTypeConfiguration<ProductWarehouse>
    {
        public void Configure(EntityTypeBuilder<ProductWarehouse> builder)
        {


            builder
                .HasKey(pw => new { pw.ProductId, pw.WarehouseId }); 

            builder
                .HasOne(pw => pw.Product)
                .WithMany(p => p.ProductWarehouse)
                .HasForeignKey(pw => pw.ProductId);

            builder
                .HasOne(pw => pw.Warehouse)
                .WithMany(w => w.ProductWarehouse)
                .HasForeignKey(pw => pw.WarehouseId);
        }
    }
}
