using RssStore.Core.Data;
using RssStore.Sales.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RssStore.Sales.Domain.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order> GetOrderById(Guid id);
        Task<IEnumerable<Order>> GetOrdersByClientId(Guid id);
        Task<Order> GetDraftOrderByClientId(Guid id);
        void AddOrder(Order order);
        void UpdateOrder(Order order);

        Task<OrderItem> GetOrderItemById(Guid id);
        Task<OrderItem> GetOrderItemByOrderId(Guid orderId, Guid productId);
        void AddOrderItem(OrderItem orderItem);
        void UpdateOrderItem(OrderItem orderItem);
        void RemoveOrderItem(OrderItem orderItem);

        Task<Voucher> GetVoucherByCode(string code);
    }
}
