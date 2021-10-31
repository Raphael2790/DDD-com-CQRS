using FluentValidation;
using RssStore.Sales.Domain.Entities;
using System;

namespace RssStore.Sales.Domain.EntityValidations
{
    public class OrderValidation : AbstractValidator<Order>
    {
        public static string ClientIdErrorMsg => "O identificador do cliente deve ser válido";
        public static string OrderStatusErrorMsg => "O pedido deve possuir um status desde o inicio";
        public static string OrderTotalValueErrorMsg => "O pedido não pode ter valor menor que zero";

        public OrderValidation()
        {
            RuleFor(x => x.ClientId)
                .NotEqual(Guid.Empty)
                .WithMessage(ClientIdErrorMsg)
                .NotNull()
                .WithMessage(ClientIdErrorMsg);

            RuleFor(x => x.OrderStatus)
                .IsInEnum()
                .WithMessage(OrderStatusErrorMsg);

            RuleFor(x => x.TotalValue)
                .LessThan(0)
                .WithMessage(OrderTotalValueErrorMsg);

            RuleFor(x => x.VoucherApplyed)
                .Equal(false)
                .When(x => x.VoucherId is null);
        }
    }
}
