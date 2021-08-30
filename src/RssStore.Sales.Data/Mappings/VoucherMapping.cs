using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RssStore.Sales.Domain.Entities;

namespace RssStore.Sales.Data.Mappings
{
    public class VoucherMapping : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {
            builder.HasKey(v => v.Id);

            builder.ToTable("Vouchers");

            builder.Property(v => v.Code)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(v => v.Percentual)
                .HasColumnType("decimal(10,2)");

            builder.Property(v => v.DiscountValue)
               .HasColumnType("decimal(10,2)");

            builder.Property(v => v.Amount)
               .HasColumnType("decimal(10,2)");

            builder.Property(v => v.VoucherDiscountType)
                .HasConversion<int>();

            builder.Property(v => v.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(v => v.ApplyedAt)
                .HasColumnType("datetime");

            builder.Property(v => v.ExpirationDate)
                .HasColumnType("datetime");

            builder.Property(v => v.Active)
                .HasColumnType("bit");

            builder.Property(v => v.Applyed)
                .HasColumnType("bit");

            builder.HasMany(v => v.Orders)
                .WithOne(o => o.Voucher)
                .HasForeignKey(o => o.VoucherId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
