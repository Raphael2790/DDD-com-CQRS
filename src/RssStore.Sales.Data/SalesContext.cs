using Microsoft.EntityFrameworkCore;
using RssStore.Core.Communication.Mediator;
using RssStore.Core.DomainObjects.Messages;
using RssStore.Sales.Data.Extensions;
using RssStore.Sales.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RssStore.Sales.Data
{
    public class SalesContext : DbContext
    {
        private readonly IMediatorHandler _mediatorHandler;
        public SalesContext(DbContextOptions<SalesContext> options, IMediatorHandler mediatorHandler) : base(options)
        {
            _mediatorHandler = mediatorHandler;
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.Ignore<Event>();

            modelBuilder.HasSequence<int>("MySequence").StartsAt(1000).IncrementsBy(1);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SalesContext).Assembly);
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

            //Pegando todos os eventos de mudanças dentro das entidades pelo change tracker

            var success = await base.SaveChangesAsync() > 0;

            if(success)
                await _mediatorHandler.PublishEvents(this);

            return success;
        }
    }
}
