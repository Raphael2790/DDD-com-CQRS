using FluentValidation;
using RssStore.Sales.Application.Commands;
using System;

namespace RssStore.Sales.Application.Validations
{
    public class UpdateOrderItemValidation : AbstractValidator<UpdateOrderItemCommand>
    {
        public UpdateOrderItemValidation()
        {
            RuleFor(x => x.ClientId)
                .NotEqual(Guid.Empty)
                .WithMessage("O Id do cliente é inválido");

            RuleFor(x => x.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage("O Id do produto é inválido");

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("A quantidade miníma de um item é 1");

            RuleFor(x => x.Amount)
                .LessThanOrEqualTo(15)
                .WithMessage("A quantidade máxima de um item é 15");
        }
    }
}
