using RssStore.Core.DomainObjects.DTOs;
using System;

namespace RssStore.Core.DomainObjects.Messages.CommonMessages.IntegrationEvents
{
    public class OrderProcessCancelledEvent : IntegrationEvent
    {
        public OrderProcessCancelledEvent(Guid orderId, Guid clientId, ProductsOrderList productOrderList)
        {
            AggregateId = orderId;
            OrderId = orderId;
            ClientId = clientId;
            ProductOrderList = productOrderList;
        }

        public Guid OrderId { get; }
        public Guid ClientId { get; }
        public ProductsOrderList ProductOrderList { get; }
    }
}
