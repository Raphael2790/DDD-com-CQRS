using Microsoft.EntityFrameworkCore;
using RssStore.Catalog.Data.Extensions;
using RssStore.Catalog.Domain.Entities;
using RssStore.Core.Communication.Mediator;
using RssStore.Core.Data;
using RssStore.Core.DomainObjects.Messages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RssStore.Catalog.Data
{
    public class CatalogContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler _mediatorHandler;
        public CatalogContext(DbContextOptions<CatalogContext> options, IMediatorHandler mediatorHandler) : base(options)
        {
            _mediatorHandler = mediatorHandler;
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.Ignore<Event>();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogContext).Assembly);
        }

        public async Task<bool> Commit()
        {
            foreach(var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("RegisterDate") != null))
            {
                if (entry.State == EntityState.Added)
                    entry.Property("RegisterDate").CurrentValue = DateTime.Now;

                if (entry.State == EntityState.Modified)
                    entry.Property("RegisterDate").IsModified = false;
            }

            var success = await base.SaveChangesAsync() > 0;
            if (success)
                await _mediatorHandler.PublishEvents(this);

            return success;
        }
    }
}
