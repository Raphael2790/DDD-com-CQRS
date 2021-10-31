using RssStore.Sales.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace RssStore.Sales.Domain.Tests.Fixtures
{
    [CollectionDefinition(nameof(OrderTestsCollention))]
    public class OrderTestsCollention : ICollectionFixture<OrderTestsFixture> { }

    public class OrderTestsFixture : IDisposable
    {
        public Order DraftOrder() => Order.OrderFactory.NewDrafOrder(Guid.NewGuid());

        public void Dispose()
        {
            
        }
    }
}
