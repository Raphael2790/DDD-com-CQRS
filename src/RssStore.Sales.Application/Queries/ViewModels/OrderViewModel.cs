using System;

namespace RssStore.Sales.Application.Queries.ViewModels
{
    public class OrderViewModel
    {
        public int Code { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime RegisterDate { get; set; }
        public int OrderStatus { get; set; }

    }
}
