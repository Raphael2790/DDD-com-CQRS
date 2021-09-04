using RssStore.Core.BaseEntity.DomainObjects;
using RssStore.Core.Interfaces.DomainObjects;
using System;

namespace RssStore.Payment.Business.Entities
{
    public class Payment : Entity, IAggregateRoot
    {

        public Guid OrderId { get; private set; }
        public string Status { get; private set; }
        public decimal Value { get; private set; }
        public string CardName { get; private set; }
        public string  CardNumber { get; private set; }
        public string CardExpirationDate { get; private set; }
        public string CardCvv { get; private set; }

        public Payment(Guid orderId, decimal value, string cardName, string cardNumber, string cardExpirationDate, string cardCvv)
        {
            OrderId = orderId;
            Value = value;
            CardName = cardName;
            CardNumber = cardNumber;
            CardExpirationDate = cardExpirationDate;
            CardCvv = cardCvv;
        }

        protected Payment() { }

        //EF Relation
        public virtual Transaction Transaction { get; set; }
    }
}
