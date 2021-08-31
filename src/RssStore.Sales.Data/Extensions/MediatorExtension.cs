using RssStore.Core.BaseEntity.DomainObjects;
using RssStore.Core.Communication.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssStore.Sales.Data.Extensions
{
    public static class MediatorExtension
    {
        public static async Task PublishEvents(this IMediatorHandler mediatorHandler, SalesContext context)
        {
            var domainEntities = context.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.NoticationEvents != null && x.Entity.NoticationEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.NoticationEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(x => x.Entity.ClearEvents());

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await mediatorHandler.PublishEvent(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}
