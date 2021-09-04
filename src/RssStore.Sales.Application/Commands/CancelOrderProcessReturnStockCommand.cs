using RssStore.Core.DomainObjects.Messages;
using System;

namespace RssStore.Sales.Application.Commands
{
    public class CancelOrderProcessReturnStockCommand : Command
    {
        public CancelOrderProcessReturnStockCommand(Guid orderId, Guid clientId)
        {
            AggregateId = orderId;
            OrderId = orderId;
            ClientId = clientId;
        }

        public Guid OrderId { get; }
        public Guid ClientId { get; }
    }
}
