using MediatR;
using RssStore.Core.DomainObjects.Messages;
using System;

namespace RssStore.Core.Communication.DomainEvents
{
    public abstract class DomainEvent : Message, INotification
    {
        public DateTime TimeStamp { get; set; }
        public DomainEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
            TimeStamp = DateTime.Now;
        }
    }
}
