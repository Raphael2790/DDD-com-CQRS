using RssStore.Core.DomainObjects.Messages;
using RssStore.Sales.Application.Validations;
using System;

namespace RssStore.Sales.Application.Commands
{
    public class AddOrderItemCommand : Command
    {
        public AddOrderItemCommand(Guid clientId, Guid productId, string productName, int amount, decimal unitValue)
        {
            ClientId = clientId;
            ProductId = productId;
            ProductName = productName;
            Amount = amount;
            UnitValue = unitValue;
        }

        public Guid ClientId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Amount { get; private set; }
        public decimal UnitValue { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new AddOrderItemValidation().Validate(this);
            return ValidationResult.IsValid;
        }

    }
}
