using RssStore.Core.DomainObjects.Messages;
using RssStore.Sales.Application.Validations;
using System;

namespace RssStore.Sales.Application.Commands
{
    public class IniciateOrderCommand : Command
    {

        public Guid OrderId { get; private set; }
        public Guid ClientId { get; private set; }
        public decimal Total { get; private set; }
        public string CardName { get; private set; }
        public string CardNumber { get; private set; }
        public string CardExpirationDate { get; private set; }
        public string CardCvv { get; private set; }

        public IniciateOrderCommand(Guid orderId, Guid clientId, decimal total, string cardName, string cardNumber, string cardExpirationDate, string cardCvv)
        {
            OrderId = orderId;
            ClientId = clientId;
            Total = total;
            CardName = cardName;
            CardNumber = cardNumber;
            CardExpirationDate = cardExpirationDate;
            CardCvv = cardCvv;
        }

        public override bool IsValid()
        {
            ValidationResult = new IniciateOrderValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
