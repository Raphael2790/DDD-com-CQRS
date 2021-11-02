using RssStore.Core.DomainObjects;
using RssStore.Sales.Domain.Entities;
using RssStore.Sales.Domain.Enums;
using RssStore.Sales.Domain.Tests.Fixtures;
using System;
using System.Linq;
using Xunit;

namespace RssStore.Sales.Domain.Tests
{
    [Collection(nameof(OrderTestsCollention))]
    public class OrderTests
    {
        private readonly OrderTestsFixture _orderFixture;
        public OrderTests(OrderTestsFixture orderFixture)
        {
            _orderFixture = orderFixture;
        }

        [Fact(DisplayName = "Adicionar Item novo pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AddItem_WhenIsANewOrder_ShouldUpdateOrderValue()
        {
            //Arrange
            var order = _orderFixture.DraftOrder();
            var orderItem = new OrderItem(Guid.NewGuid(), "Produto Teste", 1, 200);

            //Act
            order.AddItem(orderItem);

            //Assert
            Assert.Equal(200, order.TotalValue);
            Assert.Equal(1, order.OrderItems.Count);
        }

        [Fact(DisplayName = "Adicionar Item pedido existente")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AddItem_WhenItemExistsInOrder_ShouldIncrementQuantityAndUpdateOrderValue()
        {
            //Arrange
            var order = _orderFixture.DraftOrder();
            var productId = Guid.NewGuid();
            var orderItem1 = new OrderItem(productId, "Produto teste", 1, 100);
            order.AddItem(orderItem1);
            var orderItem2 = new OrderItem(productId, "Produto teste", 2, 100);

            //Act
            order.AddItem(orderItem2);

            //Assert
            Assert.Equal(300, order.TotalValue);
            Assert.Equal(1, order.OrderItems.Count);
            Assert.Equal(3, order.OrderItems.Where(x => x.ProductId == productId).FirstOrDefault().Quantity);
        }

        [Fact(DisplayName = "Adicionar Item quantidade acima do permitido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AddItem_WhenItemAmountAbovePermitedLimit_ShouldThrowDomainException()
        {
            //Arrange
            var order = _orderFixture.DraftOrder();
            var productId = Guid.NewGuid();
            var orderItem1 = new OrderItem(productId, "Produto teste", Order.MAX_UNIT_ITEM + 1, 100);

            //Act && Assert
            var erro = Assert.Throws<DomainException>(() => order.AddItem(orderItem1));
            Assert.Equal($"Máximo de {Order.MAX_UNIT_ITEM} unidades por produto", erro.Message);
        }

        [Fact(DisplayName = "Adicionar item pedido existente quantidade acima do permitido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AddItem_WhenOrderExistsAddSameItemAbovePermitedLimit_ShouldThrowDomainException()
        {
            //Arrange
            var order = _orderFixture.DraftOrder();
            var productId = Guid.NewGuid();
            var orderItem1 = new OrderItem(productId, "Produto teste", 1, 100);
            order.AddItem(orderItem1);
            var orderItem2 = new OrderItem(productId, "Produto teste", Order.MAX_UNIT_ITEM, 100);

            //Act && Assert
            var error = Assert.Throws<DomainException>(() => order.AddItem(orderItem2));
            Assert.Equal($"Máximo de {Order.MAX_UNIT_ITEM} unidades por produto", error.Message);
        }

        [Fact(DisplayName = "Atualizar item pedido item não existe no pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void UpdateItem_WhenItemNotExistsInOrder_ShouldThrowDomainException()
        {
            //Arrange
            var order = _orderFixture.DraftOrder();
            var productId = Guid.NewGuid();
            var orderItem1 = new OrderItem(productId, "Produto teste", 1, 100);

            //Act && Assert
            var error = Assert.Throws<DomainException>(() => order.UpdateItem(orderItem1));
            Assert.Equal("O item não pertence ao pedido", error.Message);
        }

        [Fact(DisplayName = "Atualizar item pedido deve atualizar quantidade item")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void UpdateItem_WhenItemExistsInOrder_ShouldUpdateOrderQuantityItem()
        {
            //Arrange
            var order = _orderFixture.DraftOrder();
            var productId = Guid.NewGuid();
            var orderItem1 = new OrderItem(productId, "Produto teste", 1, 100);
            var orderItem2 = new OrderItem(Guid.NewGuid(), "Produto xpto", 1, 50);
            order.AddItem(orderItem1);
            order.AddItem(orderItem2);
            var updatedItem = new OrderItem(productId, "Produto teste", 5, 100);
            var updateQuantity = updatedItem.Quantity;

            //Act
            order.UpdateItem(updatedItem);

            //Assert
            Assert.Equal(updateQuantity, order.OrderItems.Where(x => x.ProductId == updatedItem.ProductId).FirstOrDefault().Quantity);
        }

        [Fact(DisplayName = "Atualizar item pedido deve atualizar valor total pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void UpdateItem_WhenItemExistsInOrder_ShouldUpdateOrderTotalValue()
        {
            var order = _orderFixture.DraftOrder();
            var productId = Guid.NewGuid();
            var orderItem1 = new OrderItem(productId, "Produto teste", 1, 100);
            var orderItem2 = new OrderItem(Guid.NewGuid(), "Produto xpto", 1, 50);
            order.AddItem(orderItem1);
            order.AddItem(orderItem2);
            var updatedItem = new OrderItem(productId, "Produto teste", 5, 100);
            var updatedOrderTotal = (orderItem2.Quantity * orderItem2.UnitValue) + 
                                     (updatedItem.Quantity * updatedItem.UnitValue);

            //Act
            order.UpdateItem(updatedItem);

            //Assert
            Assert.Equal(updatedOrderTotal, order.TotalValue);
        }

        [Fact(DisplayName = "Atualizar item pedido quantidade acima permitida")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void UpdateItem_WhenItemExistsInOrderButUpdateIsOverQuantityLimit_ShouldThrowDomainException()
        {
            //Arrange
            var order = _orderFixture.DraftOrder();
            var productId = Guid.NewGuid();
            var orderItem1 = new OrderItem(productId, "Produto teste", 1, 100);
            order.AddItem(orderItem1);
            var updatedItem = new OrderItem(productId, "Produto teste", Order.MAX_UNIT_ITEM + 1, 100);

            //Act && Assert
            var error = Assert.Throws<DomainException>(() => order.UpdateItem(updatedItem));
            Assert.Equal($"Máximo de {Order.MAX_UNIT_ITEM} unidades por produto", error.Message);
        }

        [Fact(DisplayName = "Remover item inexistente no pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void RemoveItem_WhenItemNotExistsInOrder_ShouldThrowDomainException()
        {
            //Arrange
            var order = _orderFixture.DraftOrder();
            var productId = Guid.NewGuid();
            var orderItem1 = new OrderItem(productId, "Produto teste", 1, 100);

            //Act && Assert
            var error = Assert.Throws<DomainException>(() => order.RemoveItem(orderItem1));
            Assert.Equal("O item não pertence ao pedido", error.Message);
        }

        [Fact(DisplayName = "Remover item pedido deve atualizar valor pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void RemoveItem_WhenItemExistsInOrder_ShouldUpdateOrderTotalValue()
        {
            //Arrange
            var order = _orderFixture.DraftOrder();
            var productId = Guid.NewGuid();
            var orderItem1 = new OrderItem(productId, "Produto teste", 1, 100);
            var orderItem2 = new OrderItem(Guid.NewGuid(), "Produto xpto", 1, 50);
            order.AddItem(orderItem1);
            order.AddItem(orderItem2);
            var orderTotal = (orderItem1.Quantity * orderItem1.UnitValue);

            //Act
            order.RemoveItem(orderItem2);

            //Assert
            Assert.Equal(orderTotal, order.TotalValue);
            Assert.Equal(1, order.OrderItems.Count);
        }

        [Fact(DisplayName = "Pedido aplicar voucher válido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void ApplyVoucher_WhenVoucherIsValid_ShouldApplyWithSuccess()
        {
            //Arrange
            var order = _orderFixture.DraftOrder();
            var voucher = new Voucher("PROMO-15", null, 15, 10, VoucherDiscountType.Value, DateTime.Now.AddDays(15), true, false);

            //Act
            var result = order.ApplyVoucher(voucher);

            //Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact(DisplayName = "Pedido aplicar voucher inválido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void ApplyVoucher_WhenVoucherIsInvalid_ShouldReturnErrors()
        {
            //Arrange
            var order = _orderFixture.DraftOrder();
            var voucher = new Voucher("", null, null, 0, VoucherDiscountType.Value, DateTime.Now.AddDays(-1), false, true);

            //Act
            var result = order.ApplyVoucher(voucher);

            //Assert
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
        }

        [Fact(DisplayName = "Pedido aplicar voucher tipo valor")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void ApplyVoucher_WhenVoucherIsValidAndIsValueType_ShouldDiscountOrderTotalValue()
        {
            //Arrange
            var order = _orderFixture.DraftOrder();
            var productId = Guid.NewGuid();
            var orderItem1 = new OrderItem(productId, "Produto teste", 1, 100);
            var orderItem2 = new OrderItem(Guid.NewGuid(), "Produto xpto", 1, 50);
            var voucher = new Voucher("PROMO-15", null, 15, 10, VoucherDiscountType.Value, DateTime.Now.AddDays(15), true, false);
            order.AddItem(orderItem1);
            order.AddItem(orderItem2);
            var orderWithDiscount = order.TotalValue - voucher.DiscountValue;

            //Act
            order.ApplyVoucher(voucher);

            //Assert
            Assert.Equal(orderWithDiscount, order.TotalValue);
            Assert.Equal(voucher.DiscountValue, order.Discount);
        }

        [Fact(DisplayName = "Pedido aplicar voucher tipo desconto")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void ApplyVoucher_WhenVoucherIsValidAndIsPercentageType_ShouldDiscountOrderTotalValue()
        {
            //Arrange
            var order = _orderFixture.DraftOrder();
            var productId = Guid.NewGuid();
            var orderItem1 = new OrderItem(productId, "Produto teste", 1, 100);
            var orderItem2 = new OrderItem(Guid.NewGuid(), "Produto xpto", 1, 50);
            var voucher = new Voucher("PROMO-15", 15, null, 10, VoucherDiscountType.Percentage, DateTime.Now.AddDays(15), true, false);
            order.AddItem(orderItem1);
            order.AddItem(orderItem2);
            var discount = (order.TotalValue * voucher.Percentual) / 100;
            var orderTotal = order.TotalValue - discount;

            //Act
            order.ApplyVoucher(voucher);

            //Assert
            Assert.Equal(orderTotal, order.TotalValue);
            Assert.Equal(discount, order.Discount);
        }

        [Fact(DisplayName = "Pedido aplicar voucher desconto excede valor pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void ApplyVoucher_WhenVoucherDiscountIsGreaterThanOrderTotalValue_OrderTotalShouldBeZero()
        {
            //Arrange
            var order = _orderFixture.DraftOrder();
            var productId = Guid.NewGuid();
            var orderItem1 = new OrderItem(productId, "Produto teste", 1, 100);
            var orderItem2 = new OrderItem(Guid.NewGuid(), "Produto xpto", 1, 50);
            var voucher = new Voucher("PROMO-FREE", null, 200, 10, VoucherDiscountType.Value, DateTime.Now.AddDays(15), true, false);
            order.AddItem(orderItem1);
            order.AddItem(orderItem2);

            //Act
            order.ApplyVoucher(voucher);

            //Assert
            Assert.Equal(0, order.TotalValue);
        }

        [Fact(DisplayName = "Pedido aplicar vocuher adicionar item apos aplicar voucher")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void ApplyVoucher_WhenAddedItemAfterApplyVoucher_ShouldCalculeOrderTotalValue()
        {
            //Arrange
            var order = _orderFixture.DraftOrder();
            var productId = Guid.NewGuid();
            var orderItem1 = new OrderItem(productId, "Produto teste", 1, 100);
            var orderItem2 = new OrderItem(Guid.NewGuid(), "Produto xpto", 1, 50);
            var voucher = new Voucher("PROMO-15", null, 15, 10, VoucherDiscountType.Value, DateTime.Now.AddDays(15), true, false);
            order.AddItem(orderItem2);
            order.ApplyVoucher(voucher);

            //Act
            order.AddItem(orderItem1);

            //Assert
            var orderTotalExpected = order.OrderItems.Sum(x => x.Quantity * x.UnitValue) - voucher.DiscountValue;
            Assert.Equal(orderTotalExpected, order.TotalValue);
        }
    }
}
