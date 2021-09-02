using RssStore.Core.DomainObjects.Messages;
using System;

namespace RssStore.Sales.Application.Events
{
    //Eventos sempre devem conter o id da agregação
    public class DraftOrderInitializedEvent : Event
    {
        public DraftOrderInitializedEvent(Guid clientId, Guid orderId)
        {
            AggregateId = orderId;
            ClientId = clientId;
            OrderId = orderId;
        }

        public Guid ClientId { get; private set; }
        public Guid OrderId { get; private set; }

    }
}
