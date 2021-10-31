using RssStore.Core.BaseEntity.DomainObjects;
using RssStore.Sales.Domain.EntityValidations;
using System;

namespace RssStore.Sales.Domain.Entities
{
    public class OrderItem : Entity
    {
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitValue { get; private set; }

        //EF Relation
        public Order Order { get; private set; } 

        
        protected OrderItem() { }

        public OrderItem(Guid productId, string productName, int amount, decimal unitValue)
        {
            ProductId = productId;
            ProductName = productName;
            Quantity = amount;
            UnitValue = unitValue;
        }

        internal void AssociateOrder(Guid orderId)
        {
            OrderId = orderId;
        }

        public decimal CalculateValue()
        {
            return Quantity * UnitValue;
        }

        internal void AddAmount(int amount)
        {
            Quantity += amount;
        }

        internal void UpdateAmount(int amount)
        {
            Quantity = amount;
        }

        public override bool IsValid()
        {
            return new OrderItemValidation().Validate(this).IsValid;
        }
    }
}
