using RssStore.Core.DomainObjects.Messages;
using System;

namespace RssStore.Sales.Application.Events
{
    //Eventos sempre devem conter o id da agregação para identicar a raiz de agregação
    public class ProductOrderUpdatedEvent : Event
    {

        public Guid ClientId { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public int Amount { get; private set; }

        public ProductOrderUpdatedEvent(Guid clientId, Guid orderId, Guid productId, int amount)
        {
            AggregateId = orderId;
            ClientId = clientId;
            OrderId = orderId;
            ProductId = productId;
            Amount = amount;
        }
    }
}
