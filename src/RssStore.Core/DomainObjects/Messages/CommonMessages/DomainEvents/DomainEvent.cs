using RssStore.Core.DomainObjects.Messages;
using System;

namespace RssStore.Core.Communication.DomainEvents
{
    public class DomainEvent : Event
    {
        public DomainEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }
    }
}
