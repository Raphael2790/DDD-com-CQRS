using System;
using System.Collections.Generic;

namespace RssStore.Core.DomainObjects.DTOs
{
    public class ProductsOrderList
    {
        public Guid OrderId { get; set; }
        public ICollection<Item> Items { get; set; }

    }

    public class Item
    {
        public Guid Id { get; set; }
        public int Amount { get; set; }
    }
}
