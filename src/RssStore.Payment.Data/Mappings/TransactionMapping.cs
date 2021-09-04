using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RssStore.Payment.Business.Entities;

namespace RssStore.Payment.Data.Mappings
{
    public class TransactionMapping : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transactions");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Total)
                .HasColumnType("decimal(10,2)");

            builder.Property(t => t.TransactionStatus)
                .HasConversion<int>();
        }
    }
}
