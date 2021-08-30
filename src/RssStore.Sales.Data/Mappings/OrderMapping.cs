using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RssStore.Sales.Domain.Entities;

namespace RssStore.Sales.Data.Mappings
{
    public class OrderMapping : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.ToTable("Orders");

            builder.Property(o => o.Code)
                .HasDefaultValueSql("NEXT VALUE FOR MySequence")
                .HasMaxLength(10);

            builder.Property(o => o.VoucherApplyed)
                .HasColumnType("bit");

            builder.Property(o => o.Discount)
                .HasColumnType("decimal(10,2)");

            builder.Property(o => o.TotalValue)
                .HasColumnType("decimal(10,2)");

            builder.Property(o => o.RegisterDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(o => o.OrderStatus)
                .HasConversion<int>();

            builder.HasMany(o => o.OrderItems)
                .WithOne(o => o.Order)
                .HasForeignKey(o => o.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
