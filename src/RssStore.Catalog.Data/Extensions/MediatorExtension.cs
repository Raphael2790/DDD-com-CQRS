using RssStore.Core.BaseEntity.DomainObjects;
using RssStore.Core.Communication.Mediator;
using System.Linq;
using System.Threading.Tasks;

namespace RssStore.Catalog.Data.Extensions
{
    public static class MediatorExtension
    {
        public static async Task PublishEvents(this IMediatorHandler mediatorHandler, CatalogContext context)
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
