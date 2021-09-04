using RssStore.Core.DomainObjects.Messages;
using System;

namespace RssStore.Sales.Application.Events
{
    //O evento poderia ser de integração caso alguém fosse interessado
    public class FinishedOrderEvent : Event
    {
        public FinishedOrderEvent(Guid orderId)
        {
            AggregateId = orderId;
            OrderId = orderId;
        }

        public Guid OrderId { get; }
    }
}
