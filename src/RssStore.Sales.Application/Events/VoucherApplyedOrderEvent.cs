using RssStore.Core.DomainObjects.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace RssStore.Sales.Application.Events
{
    public class VoucherApplyedOrderEvent : Event
    {

        public Guid ClientId { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid VoucherId { get; private set; }

        public VoucherApplyedOrderEvent(Guid clientId, Guid orderId, Guid voucherId)
        {
            ClientId = clientId;
            OrderId = orderId;
            VoucherId = voucherId;
        }
    }
}
