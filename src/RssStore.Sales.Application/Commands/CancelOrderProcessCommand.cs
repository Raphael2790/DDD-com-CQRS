using RssStore.Core.DomainObjects.Messages;
using System;

namespace RssStore.Sales.Application.Commands
{
    public class CancelOrderProcessCommand : Command
    {
        public CancelOrderProcessCommand(Guid orderId, Guid clientId)
        {
            OrderId = orderId;
            ClientId = clientId;
        }

        public Guid OrderId { get; }
        public Guid ClientId { get; }
    }
}
