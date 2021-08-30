using RssStore.Core.Communication.DomainEvents;
using System;

namespace RssStore.Catalog.Domain.Events
{
    public class LowProductStockEvent : DomainEvent
    {
        public int LowAmount { get; private set; }
        public LowProductStockEvent(Guid aggregateId, int lowAmount) : base(aggregateId)
        {
            LowAmount = lowAmount;
        }
    }
}
