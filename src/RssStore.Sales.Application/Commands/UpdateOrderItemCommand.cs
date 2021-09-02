using RssStore.Core.DomainObjects.Messages;
using RssStore.Sales.Application.Validations;
using System;

namespace RssStore.Sales.Application.Commands
{
    public class UpdateOrderItemCommand : Command
    {
        public UpdateOrderItemCommand(Guid clientId, Guid productId, int amount)
        {
            ClientId = clientId;
            ProductId = productId;
            Amount = amount;
        }

        public Guid ClientId { get; private set; }
        public Guid ProductId { get; private set; }
        public int Amount { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new UpdateOrderItemValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

}
