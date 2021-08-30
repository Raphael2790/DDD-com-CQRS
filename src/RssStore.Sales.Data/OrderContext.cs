using Microsoft.EntityFrameworkCore;
using RssStore.Core.DomainObjects.Messages;
using RssStore.Sales.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RssStore.Sales.Data
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options){}

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.Ignore<Event>();

            modelBuilder.HasSequence<int>("MySequence").StartsAt(1000).IncrementsBy(1);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderContext).Assembly);
        }

        public async Task<bool> Commit()
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("RegisterDate") != null))
            {
                if (entry.State == EntityState.Added)
                    entry.Property("RegisterDate").CurrentValue = DateTime.Now;

                if (entry.State == EntityState.Modified)
                    entry.Property("RegisterDate").IsModified = false;
            }

            return await base.SaveChangesAsync() > 0;
        }
    }
}
