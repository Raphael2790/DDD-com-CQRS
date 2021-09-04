using FluentValidation;
using RssStore.Sales.Application.Commands;
using System;

namespace RssStore.Sales.Application.Validations
{
    public class IniciateOrderValidation : AbstractValidator<IniciateOrderCommand>
    {
        public IniciateOrderValidation()
        {
            RuleFor(x => x.ClientId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do cliente é inválido");

            RuleFor(x => x.OrderId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do pedido é inválido");

            RuleFor(x => x.CardName)
                .NotEmpty()
                .WithMessage("O nome do cartão não foi informado");

            RuleFor(x => x.CardNumber)
                .CreditCard()
                .WithMessage("Número de cartão de crédito inválido");

            RuleFor(x => x.CardExpirationDate)
                .NotEmpty()
                .WithMessage("Data de expiração não informada");

            RuleFor(x => x.CardCvv)
                .Length(3, 4)
                .WithMessage("O CVV não foi preenchido corretamente");
        }
    }
}
