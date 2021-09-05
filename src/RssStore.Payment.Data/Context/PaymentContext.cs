using Microsoft.EntityFrameworkCore;
using RssStore.Core.Communication.Mediator;
using RssStore.Core.Data;
using RssStore.Core.DomainObjects.Messages;
using RssStore.Payment.Business.Entities;
using RssStore.Payment.Data.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Payments = RssStore.Payment.Business.Entities.Payment;

namespace RssStore.Payment.Data.Context
{
    public class PaymentContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler _mediatorHandler;
        public PaymentContext(DbContextOptions<PaymentContext> options, IMediatorHandler mediatorHandler) : base(options)
        {
            _mediatorHandler = mediatorHandler;
        }

        public DbSet<Payments> Payments { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public async Task<bool> Commit()
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("RegisterDate") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("RegisterDate").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("RegisterDate").IsModified = false;
                }
            }

            var success = await base.SaveChangesAsync() > 0;
            if (success) await _mediatorHandler.PublishEvents(this);

            return success;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Ignore<Event>();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentContext).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
            base.OnModelCreating(modelBuilder);
        }
    }
}
