using FluentValidation;
using RssStore.Sales.Application.Commands;
using System;

namespace RssStore.Sales.Application.Validations
{
    public class RemoveOrderItemValidation : AbstractValidator<RemoveOrderItemCommand>
    {
        public RemoveOrderItemValidation()
        {
            RuleFor(x => x.ClientId)
                .NotEqual(Guid.Empty)
                .WithMessage("O Id do cliente é inválido");

            RuleFor(x => x.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage("O Id do produto é inválido");
        }
    }
}
