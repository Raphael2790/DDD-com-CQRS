using RssStore.Core.DomainObjects.Messages;
using System;

namespace RssStore.Sales.Application.Commands
{
    public class FinishOrderCommand : Command
    {
        public FinishOrderCommand(Guid orderId, Guid clientId)
        {
            AggregateId = orderId;
            OrderId = orderId;
            ClientId = clientId;
        }

        public Guid OrderId { get; }
        public Guid ClientId { get; }
    }
}
