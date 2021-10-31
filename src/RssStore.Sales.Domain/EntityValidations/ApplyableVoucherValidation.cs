using FluentValidation;
using RssStore.Sales.Domain.Entities;
using RssStore.Sales.Domain.Enums;
using System;

namespace RssStore.Sales.Domain.EntityValidations
{
    public class ApplyableVoucherValidation : AbstractValidator<Voucher>
    {
        public static string CodeErrorMsg => "Voucher sem código válido";
        public static string ExpirationDateErrorMsg => "Este voucher está expirado";
        public static string ActiveErrorMsg => "Este voucher não é mais válido";
        public static string ApplyedErrorMsg => "Este voucher já foi utilizado";
        public static string AmountErrorMsg => "Este voucher não está mais disponível";
        public static string DiscountErrorMsg => "O valor desconto precisa ser superior a 0";
        public static string PercentualDiscountErrorMsg => "O valor da percentagem de desconto precisa ser superior a 0";

        public ApplyableVoucherValidation()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage(CodeErrorMsg);

            RuleFor(x => x.ExpirationDate)
                .Must(ExpirationDateIsLongerThanActualDate)
                .WithMessage(ExpirationDateErrorMsg);

            RuleFor(x => x.Active)
                .Equal(true)
                .WithMessage(ActiveErrorMsg);

            RuleFor(x => x.Applyed)
                .Equal(false)
                .WithMessage(ApplyedErrorMsg);

            RuleFor(x => x.Amount)
                .GreaterThan(default(int))
                .WithMessage(AmountErrorMsg);

            When(x => x.VoucherDiscountType == VoucherDiscountType.Value, () =>
            {
                RuleFor(x => x.DiscountValue)
                    .NotNull()
                    .WithMessage(DiscountErrorMsg)
                    .GreaterThan(default(int))
                    .WithMessage(DiscountErrorMsg);
            });

            When(x => x.VoucherDiscountType == VoucherDiscountType.Percentage, () =>
            {
                RuleFor(x => x.Percentual)
                    .NotNull()
                    .WithMessage(PercentualDiscountErrorMsg)
                    .GreaterThan(default(int))
                    .WithMessage(PercentualDiscountErrorMsg);
            });
        }

        private bool ExpirationDateIsLongerThanActualDate(DateTime data)
        {
            return data >= DateTime.Now;
        }
    }
}
