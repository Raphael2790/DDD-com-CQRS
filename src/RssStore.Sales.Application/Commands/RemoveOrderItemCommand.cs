using RssStore.Core.DomainObjects.Messages;
using RssStore.Sales.Application.Validations;
using System;

namespace RssStore.Sales.Application.Commands
{
    public class RemoveOrderItemCommand : Command
    {
        public Guid ClientId { get; private set; }
        public Guid ProductId { get; private set; }

        public RemoveOrderItemCommand(Guid clientId, Guid productId)
        {
            ClientId = clientId;
            ProductId = productId;
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveOrderItemValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
