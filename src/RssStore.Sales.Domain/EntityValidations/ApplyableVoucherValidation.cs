using FluentValidation;
using RssStore.Sales.Domain.Entities;
using System;

namespace RssStore.Sales.Domain.EntityValidations
{
    public class ApplyableVoucherValidation : AbstractValidator<Voucher>
    {
        public ApplyableVoucherValidation()
        {
            RuleFor(x => x.ExpirationDate)
                .Must(ExpirationDateAfterTodayDate)
                .WithMessage("Este voucher está expirado");

            RuleFor(x => x.Active)
                .Equal(false)
                .WithMessage("Este voucher não é mais válido");

            RuleFor(x => x.Applyed)
                .Equal(true)
                .WithMessage("Este voucher já foi utilizado");

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Este voucher não está mais disponível");
        }

        protected static bool ExpirationDateAfterTodayDate(DateTime expirationDate)
        {
            return expirationDate >= DateTime.Now;
        }
    }
}
