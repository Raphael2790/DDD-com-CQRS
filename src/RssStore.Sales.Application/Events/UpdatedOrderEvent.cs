using RssStore.Core.DomainObjects.Messages;
using System;

namespace RssStore.Sales.Application.Events
{
    //Eventos sempre devem conter o id da agregação para identicar a raiz de agregação
    public class UpdatedOrderEvent : Event
    {
        public UpdatedOrderEvent(Guid clientId, Guid orderId, decimal totalValue)
        {
            AggregateId = orderId;
            ClientId = clientId;
            OrderId = orderId;
            TotalValue = totalValue;
        }

        public Guid ClientId { get; private set; }
        public Guid OrderId { get; private set; }
        public decimal TotalValue { get; private set; }

    }
}
