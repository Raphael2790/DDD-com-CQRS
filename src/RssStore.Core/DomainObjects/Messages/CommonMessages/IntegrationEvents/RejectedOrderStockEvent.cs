using System;
using System.Collections.Generic;
using System.Text;

namespace RssStore.Core.DomainObjects.Messages.CommonMessages.IntegrationEvents
{
    public class RejectedOrderStockEvent : IntegrationEvent
    {

        public Guid OrderId { get; private set; }
        public Guid ClientId { get; private set; }

        public RejectedOrderStockEvent(Guid orderId, Guid clientId)
        {
            AggregateId = orderId;
            OrderId = orderId;
            ClientId = clientId;
        }

    }
}
