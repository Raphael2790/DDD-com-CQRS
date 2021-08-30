using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RssStore.Sales.Domain.Entities;

namespace RssStore.Sales.Data.Mappings
{
    public class OrderItemMapping : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(o => o.Id);

            builder.ToTable("OrderItems");

            builder.Property(o => o.ProductName)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.Property(o => o.UnitValue)
                .HasColumnType("decimal(10,2)");

            builder.Property(o => o.Amount)
                .HasColumnType("decimal(10,2)");

            builder.HasOne(o => o.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(o => o.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
