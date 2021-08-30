using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RssStore.Catalog.Domain.Entities;

namespace RssStore.Catalog.Data.EntityMapping
{
    public class ProductMapping : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasColumnType("varchar(250)");

            builder.Property(p => p.Description)
                .IsRequired()
                .HasColumnName("Description")
                .HasColumnType("varchar(250)");

            builder.Property(p => p.Image)
                .IsRequired()
                .HasColumnName("Image")
                .HasColumnType("varchar(250)");

            builder.Property(p => p.Value)
                .HasColumnType("decimal(18,2)")
                .HasColumnName("Value");

            builder.Property(p => p.Active)
                .HasColumnName("Active")
                .HasColumnType("bit");

            builder.OwnsOne(c => c.Dimensions, cm => {
                cm.Property(d => d.Width)
                .HasColumnType("int")
                .HasColumnName("Width");

                cm.Property(d => d.Height)
                .HasColumnType("int")
                .HasColumnName("Height");

                cm.Property(d => d.Depth)
                .HasColumnType("int")
                .HasColumnName("Depth");
            });
        }
    }
}
