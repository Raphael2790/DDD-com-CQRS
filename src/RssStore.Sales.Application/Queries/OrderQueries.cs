using RssStore.Sales.Application.Queries.Interfaces;
using RssStore.Sales.Application.Queries.ViewModels;
using RssStore.Sales.Domain.Enums;
using RssStore.Sales.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RssStore.Sales.Application.Queries
{
    //Classe facade para a interface pois a camada de apresentação não tem pq conhecer todos os metodos de repository
    public class OrderQueries : IOrderQueries
    {
        private readonly IOrderRepository _orderRepository;
        public OrderQueries(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<CartViewModel> GetClientCart(Guid clientId)
        {
            var order = await _orderRepository.GetDraftOrderByClientId(clientId);

            if (order is null) return null;

            var cart = new CartViewModel
            {
                ClientId = order.ClientId,
                TotalValue = order.TotalValue,
                SubTotal = order.Discount + order.TotalValue,
                DiscountValue = order.Discount,
                OrderId = order.Id
            };

            if (order.Voucher != null)
                cart.VoucherCode = order.Voucher.Code;

            foreach (var item in order.OrderItems)
            {
                cart.CartItems.Add(new CartItemViewModel
                {
                    ProductId = item.ProductId,
                    Amount = item.Amount,
                    ProductName = item.ProductName,
                    TotalValue = item.CalculateValue(),
                    UnitValue = item.UnitValue
                });
            }

            return cart;
        }

        public async Task<IEnumerable<OrderViewModel>> GetClientOrders(Guid clientId)
        {
            var orders = await _orderRepository.GetOrdersByClientId(clientId);

            orders = orders.Where(x => x.OrderStatus == OrderStatus.Payed || x.OrderStatus == OrderStatus.Cancelled)
                        .OrderByDescending(x => x.Code);

            if (!orders.Any()) return null;

            var viewOrders = new List<OrderViewModel>();

            foreach (var order in orders)
            {
                viewOrders.Add(new OrderViewModel
                {
                    TotalValue = order.TotalValue,
                    OrderStatus = (int)order.OrderStatus,
                    RegisterDate = order.RegisterDate,
                    Code = order.Code
                });
            }

            return viewOrders;
        }
    }
}
