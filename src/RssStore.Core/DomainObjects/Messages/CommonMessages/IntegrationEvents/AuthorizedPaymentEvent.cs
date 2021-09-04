using System;

namespace RssStore.Core.DomainObjects.Messages.CommonMessages.IntegrationEvents
{
    public class AuthorizedPaymentEvent: IntegrationEvent
    {
        public AuthorizedPaymentEvent(Guid orderId, Guid clientId, Guid paymentId, Guid transactionId, decimal value)
        {
            AggregateId = orderId;
            OrderId = orderId;
            ClientId = clientId;
            PaymentId = paymentId;
            TransactionId = transactionId;
            Value = value;
        }

        public Guid OrderId { get; }
        public Guid ClientId { get; }
        public Guid PaymentId { get; }
        public Guid TransactionId { get; }
        public decimal Value { get; }
    }
}
