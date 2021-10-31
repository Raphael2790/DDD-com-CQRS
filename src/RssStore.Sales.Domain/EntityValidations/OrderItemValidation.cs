using FluentValidation;
using RssStore.Sales.Domain.Entities;
using System;

namespace RssStore.Sales.Domain.EntityValidations
{
    public class OrderItemValidation : AbstractValidator<OrderItem>
    {
        public static string OrderIdErrorMsg => "O item do pedido deve possuir o identificador do pedido";
        public static string QuantityErrorMsg => "A quantidade do item deve ser maior que zero";
        public static string ProductIdErrorMsg => "O item do pedido deve possuir o identificador do produto";
        public static string ProductNameErrorMsg => "O item do pedido deve possuir o nome do produto";
        public static string UnitValueErrorMsg => "O item do pedido deve possuir o valor unitário maior que zero";

        public OrderItemValidation()
        {
            RuleFor(x => x.OrderId)
                .NotEqual(Guid.Empty)
                .WithMessage(OrderIdErrorMsg)
                .NotNull()
                .WithMessage(OrderIdErrorMsg);

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage(QuantityErrorMsg);

            RuleFor(x => x.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage(ProductIdErrorMsg)
                .NotNull()
                .WithMessage(ProductIdErrorMsg);

            RuleFor(x => x.ProductName)
                .NotNull()
                .WithMessage(ProductNameErrorMsg)
                .NotEmpty()
                .WithMessage(ProductNameErrorMsg);

            RuleFor(x => x.UnitValue)
                .GreaterThan(0)
                .WithMessage(UnitValueErrorMsg);
        }
    }
}
