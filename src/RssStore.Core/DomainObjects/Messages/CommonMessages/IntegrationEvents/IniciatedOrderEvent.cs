using RssStore.Core.DomainObjects.DTOs;
using System;

namespace RssStore.Core.DomainObjects.Messages.CommonMessages.IntegrationEvents
{
    //Cartão de credito é um otimo candidato para um Value Object
    public class IniciatedOrderEvent : IntegrationEvent
    {

        public Guid OrderId { get; private set; }
        public Guid ClientId { get; private set; }
        public decimal Total { get; private set; }
        public ProductsOrderList ProductsOrderList { get; private set; }
        public string CardName { get; private set; }
        public string CardNumber { get; private set; }
        public string CardExpirationDate { get; private set; }
        public string CardCvv { get; private set; }

        public IniciatedOrderEvent(Guid orderId, Guid clientId, decimal total, ProductsOrderList productsOrderList, string cardName, string cardNumber, string cardExpirationDate, string cardCvv)
        {
            AggregateId = orderId;
            OrderId = orderId;
            ClientId = clientId;
            Total = total;
            ProductsOrderList = productsOrderList;
            CardName = cardName;
            CardNumber = cardNumber;
            CardExpirationDate = cardExpirationDate;
            CardCvv = cardCvv;
        }

    }
}
