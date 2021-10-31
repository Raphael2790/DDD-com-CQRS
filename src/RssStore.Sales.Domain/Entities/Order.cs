using FluentValidation.Results;
using RssStore.Core.BaseEntity.DomainObjects;
using RssStore.Core.DomainObjects;
using RssStore.Core.Interfaces.DomainObjects;
using RssStore.Sales.Domain.EntityValidations;
using RssStore.Sales.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RssStore.Sales.Domain.Entities
{
    public class Order : Entity, IAggregateRoot
    {
        public static int MAX_UNIT_ITEM = 15;
        public static int MIN_UNIDADES_ITEM = 1;

        public string Code { get; private set; }
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
            RegisterDate = DateTime.Now;
        }

        public ValidationResult ApplyVoucher(Voucher voucher)
        {
            var validationResult = voucher.IsValidToApply();
            if (!validationResult.IsValid) return validationResult;

            Voucher = voucher;
            VoucherApplyed = true;
            CalculateTotalOrder();
            return validationResult;
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
            orderItem.AssociateOrder(Id);
            if (!orderItem.IsValid()) return;
            ValidateQuantityExistingItens(orderItem);

            if (OrderItemExists(orderItem))
            {
                var existsItem = _orderItems.FirstOrDefault(p => p.ProductId == orderItem.ProductId);
                existsItem.AddAmount(orderItem.Quantity);
                orderItem = existsItem;

                _orderItems.Remove(existsItem);
            }

            orderItem.CalculateValue();
            _orderItems.Add(orderItem);

            CalculateTotalOrder();
        }

        private void ValidateQuantityExistingItens(OrderItem orderItem)
        {
            var itemQuantity = orderItem.Quantity;
            if (OrderItemExists(orderItem))
            {
                var itemInOrder = _orderItems.FirstOrDefault(x => x.ProductId == orderItem.ProductId);
                itemQuantity += itemInOrder.Quantity;
            }

            if (itemQuantity > MAX_UNIT_ITEM)
                throw new DomainException($"Máximo de {MAX_UNIT_ITEM} unidades por produto");
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
                    ClientId = clientId,
                    VoucherApplyed = false
                };
                //uma vez concentrada a chamada do metodo unico ponto de mudança
                order.MakeItDraft();
                return order;
            }
        }

        public override bool IsValid()
        {
            return new OrderValidation().Validate(this).IsValid;
        }
    }
}
