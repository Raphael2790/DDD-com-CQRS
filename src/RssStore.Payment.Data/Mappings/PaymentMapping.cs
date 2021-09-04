using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RssStore.Payment.Business.Entities;
using Payments = RssStore.Payment.Business.Entities.Payment;

namespace RssStore.Payment.Data.Mappings
{
    public class PaymentMapping : IEntityTypeConfiguration<Payments>
    {
        public void Configure(EntityTypeBuilder<Payments> builder)
        {
            builder.ToTable("Payments");

            builder.HasKey(p => p.Id);

            builder.Property(c => c.CardName)
               .IsRequired()
               .HasColumnType("varchar(250)");

            builder.Property(c => c.CardNumber)
                .IsRequired()
                .HasColumnType("varchar(16)");

            builder.Property(c => c.CardExpirationDate)
                .IsRequired()
                .HasColumnType("varchar(10)");

            builder.Property(c => c.CardCvv)
                .IsRequired()
                .HasColumnType("varchar(4)");

            // 1 : 1 => Pagamento : Transacao
            builder.HasOne(c => c.Transaction)
                .WithOne(c => c.Payment)
                .HasForeignKey<Transaction>(p => p.PaymentId);

        }
    }
}
