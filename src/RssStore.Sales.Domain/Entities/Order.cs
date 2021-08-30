using RssStore.Core.BaseEntity.DomainObjects;
using RssStore.Core.DomainObjects;
using RssStore.Core.Interfaces.DomainObjects;
using RssStore.Sales.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RssStore.Sales.Domain.Entities
{
    public class Order : Entity, IAggregateRoot
    {
        public int Code { get; private set; }
        public Guid ClientId { get; private set; }
        public Guid? VoucherId { get; private set; }
        public bool VoucherApplyed { get; private set; }
        public decimal Discount { get; private set; }
        public decimal TotalValue { get; private set;}
        public DateTime RegisterDate { get; private set; }
        public OrderStatus OrderStatus { get; private set; }

        private readonly List<OrderItem> _orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        //EF Relation
        public virtual Voucher Voucher { get; private set; }

        protected Order() 
        {
            _orderItems = new List<OrderItem>();
        }

        public Order(Guid clientId, bool voucherApplyed, decimal discount, decimal totalValue)
        {
            ClientId = clientId;
            VoucherApplyed = voucherApplyed;
            Discount = discount;
            TotalValue = totalValue;
            _orderItems = new List<OrderItem>();
        }

        public void ApplyVoucher(Voucher voucher)
        {
            Voucher = voucher;
            VoucherApplyed = true;
            CalculateTotalOrder();
        }

        public void CalculateTotalOrder()
        {
            TotalValue = OrderItems.Sum(o => o.CalculateValue());
            CalculateTotalDiscount();
        }

        public void CalculateTotalDiscount()
        {
            if (!VoucherApplyed) return;

            decimal discount = 0;
            var value = TotalValue;

            if(Voucher.VoucherDiscountType == VoucherDiscountType.Percentage)
            {
                if (Voucher.Percentual.HasValue)
                {
                    discount = (value * Voucher.Percentual.Value) / 100;
                    value -= discount;
                }
            }
            else
            {
                if (Voucher.DiscountValue.HasValue)
                {
                    discount = Voucher.DiscountValue.Value;
                    value -= discount;
                }
            }

            TotalValue = value < 0 ? 0 : value;
            Discount = discount;
        }

        public bool OrderItemExists(OrderItem orderItem)
        {
            return _orderItems.Any(o => o.ProductId == orderItem.ProductId);
        }

        public void AddItem(OrderItem orderItem)
        {
            if (!orderItem.IsValid()) return;

            orderItem.AssociateOrder(Id);

            if (OrderItemExists(orderItem))
            {
                var existsItem = _orderItems.FirstOrDefault(p => p.ProductId == orderItem.ProductId);
                existsItem.AddAmount(orderItem.Amount);
                orderItem = existsItem;

                _orderItems.Remove(existsItem);
            }

            orderItem.CalculateValue();
            _orderItems.Add(orderItem);

            CalculateTotalOrder();
        }

        public void RemoveItem(OrderItem orderItem)
        {
            if (!orderItem.IsValid()) return;

            var existItem = OrderItems.FirstOrDefault(p => p.ProductId == orderItem.ProductId);

            if (existItem == null) throw new DomainException("O item não pertence ao pedido");

            _orderItems.Remove(orderItem);

            CalculateTotalOrder();
        }

        public void UpdateItem(OrderItem orderItem)
        {
            if (!orderItem.IsValid()) return;

            orderItem.AssociateOrder(Id);

            var existsItem = OrderItems.FirstOrDefault(p => p.ProductId == orderItem.ProductId);

            if (existsItem == null) throw new DomainException("O item não pertence ao pedido");

            _orderItems.Remove(existsItem);
            _orderItems.Add(orderItem);

            CalculateTotalOrder();
        }

        public void UpdateItemAmount(OrderItem orderItem, int amount)
        {
            orderItem.UpdateAmount(amount);
            UpdateItem(orderItem);
        }

        public void MakeItDraft()
        {
            OrderStatus = OrderStatus.Draft;
        }

        public void IniatializeOrder()
        {
            OrderStatus = OrderStatus.Initialized;
        }

        public void FinishOrder()
        {
            OrderStatus = OrderStatus.Payed;
        }

        public void CancelOrder()
        {
            OrderStatus = OrderStatus.Cancelled;
        }

        //Classe aninhada
        public static class OrderFactory
        {
            public static Order NewDrafOrder(Guid clientId)
            {
                var order = new Order
                {
                    ClientId = clientId
                };
                //uma vez concentrada a chamada do metodo unico ponto de mudança
                order.MakeItDraft();
                return order;
            }
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
