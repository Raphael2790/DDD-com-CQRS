using RssStore.Core.DomainObjects;
using RssStore.Sales.Domain.Entities;
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
        public void AddItem_WhenItemAmountAbovePermitedLimit_ShouldThrowDomainExeception()
        {
            //Arrange
            var order = _orderFixture.DraftOrder();
            var productId = Guid.NewGuid();
            var orderItem1 = new OrderItem(productId, "Produto teste", Order.MAX_UNIT_ITEM + 1, 100);

            //Act && Assert
            var erro = Assert.Throws<DomainException>(() => order.AddItem(orderItem1));
            Assert.Equal($"Máximo de {Order.MAX_UNIT_ITEM} unidades por produto", erro.Message);
        }
    }
}
