using RssStore.Core.DomainObjects.Messages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RssStore.Core.Data.EventSourcing.Interfaces
{
    public interface IEventSourcingRepository
    {
        Task SaveEvent<TEvent>(TEvent @event) where TEvent : Event;
        Task<IEnumerable<StoredEvent>> GetEvents(Guid aggregateId);
    }
}
