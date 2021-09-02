using FluentValidation;
using RssStore.Sales.Application.Commands;
using System;

namespace RssStore.Sales.Application.Validations
{
    public class ApplyVoucherOrderValidation : AbstractValidator<ApplyVoucherOrderCommand>
    {
        public ApplyVoucherOrderValidation()
        {
            RuleFor(x => x.ClientId)
                .NotEqual(Guid.Empty)
                .WithMessage("O Id do cliente é inválido");

            RuleFor(x => x.VoucherCode)
                .NotEmpty()
                .WithMessage("O código do voucher não pode ser vazio");
        }
    }
}
