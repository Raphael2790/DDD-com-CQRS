using System;

namespace RssStore.Sales.Application.Queries.ViewModels
{
    public class CartItemViewModel
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Amount { get; set; }
        public decimal UnitValue { get; set; }
        public decimal TotalValue { get; set; }

    }
}
