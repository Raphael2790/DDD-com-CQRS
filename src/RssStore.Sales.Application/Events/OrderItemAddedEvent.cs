using RssStore.Core.DomainObjects.Messages;
using System;

namespace RssStore.Sales.Application.Events
{
    public class OrderItemAddedEvent : Event
    {
        public OrderItemAddedEvent(Guid clientId, Guid orderId, Guid productId, decimal unitValue, int amount, string productName)
        {
            AggregateId = orderId;
            ClientId = clientId;
            OrderId = orderId;
            ProductId = productId;
            UnitValue = unitValue;
            Amount = amount;
            ProductName = productName;
        }

        public Guid ClientId { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public decimal UnitValue { get; private set; }
        public int Amount { get; private set; }
    }
}
