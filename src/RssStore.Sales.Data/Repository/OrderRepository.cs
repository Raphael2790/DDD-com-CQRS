using Microsoft.EntityFrameworkCore;
using RssStore.Core.Data;
using RssStore.Sales.Domain.Entities;
using RssStore.Sales.Domain.Enums;
using RssStore.Sales.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RssStore.Sales.Data.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderContext _context;
        public OrderRepository(OrderContext context)
        {
            _context = context;
        }
        public IUnitOfWork UnitOfWork => throw new NotImplementedException();

        public void AddOrder(Order order) => _context.Orders.Add(order);

        public void AddOrderItem(OrderItem orderItem) => _context.OrderItems.Add(orderItem);

        public async Task<Order> GetDraftOrderByClientId(Guid id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.ClientId == id && o.OrderStatus == OrderStatus.Draft);

            if (order is null) return null;

            //Carregamento de coleções sem usar inner join
            await _context.Entry(order).Collection(i => i.OrderItems).LoadAsync();

            //Carregamento de relacionamento 
            if (order.Voucher != null)
                await _context.Entry(order).Reference(v => v.Voucher).LoadAsync();

            return order;
        }

        public async Task<Order> GetOrderById(Guid id) => await _context.Orders.FindAsync(id);

        public async Task<OrderItem> GetOrderItemById(Guid id) => await _context.OrderItems.FindAsync(id);

        public async Task<OrderItem> GetOrderItemByOrderId(Guid orderId, Guid productId) => 
            await _context.OrderItems.FirstOrDefaultAsync(o => o.OrderId == orderId && o.ProductId == productId);

        public async Task<IEnumerable<Order>> GetOrdersByClientId(Guid clientId) => 
            await _context.Orders.AsNoTracking().Where(o => o.ClientId == clientId).ToListAsync();

        public async Task<Voucher> GetVoucherByCode(string code) => 
            await _context.Vouchers.Where(v => v.Code == code).FirstOrDefaultAsync();

        public void RemoveOrderItem(OrderItem orderItem) => _context.OrderItems.Remove(orderItem);

        public void UpdateOrder(Order order) => _context.Orders.Update(order);

        public void UpdateOrderItem(OrderItem orderItem) => _context.OrderItems.Update(orderItem);

        public void Dispose() => _context?.Dispose();
    }
}
