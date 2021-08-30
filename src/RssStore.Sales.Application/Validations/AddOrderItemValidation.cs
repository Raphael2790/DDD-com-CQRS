using FluentValidation;
using RssStore.Sales.Application.Commands;
using System;

namespace RssStore.Sales.Application.Validations
{
    public class AddOrderItemValidation : AbstractValidator<AddOrderItemCommand>
    {
        public AddOrderItemValidation()
        {
            RuleFor(o => o.ClientId)
                .NotEqual(Guid.Empty)
                .WithMessage("O Id do cliente é inválido");

            RuleFor(o => o.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage("O Id do produto é inválido");

            RuleFor(o => o.ProductName)
                .NotEmpty()
                .WithMessage("O nome do produto não foi informado");

            RuleFor(o => o.Amount)
                .GreaterThan(0)
                .WithMessage("A quantidade miníma de um item é 1");

            RuleFor(o => o.Amount)
                .LessThan(15)
                .WithMessage("A quantidade máxima de um item é 15");

            RuleFor(o => o.UnitValue)
                .GreaterThan(0)
                .WithMessage("O valor do item precisa ser maior que 0");


        }
    }
}
