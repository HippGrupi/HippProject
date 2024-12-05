using hippserver.Models.JunctionTables;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace hippserver.Configurations
{
    public class DriverOrderConfiguration : IEntityTypeConfiguration<DriverOrder>
    {
        public void Configure(EntityTypeBuilder<DriverOrder> builder)
        {

            builder.HasKey(op => new { op.DriverId, op.OrderId });


            builder
                .HasOne(op => op.Driver)
                .WithMany(d => d.DriverOrders)
                .HasForeignKey(dopo => dopo.DriverId);


            builder
                .HasOne(op => op.Order)
                .WithMany(o => o.DriverOrders)
                .HasForeignKey(op => op.OrderId);
        }
    }
}




