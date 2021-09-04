using RssStore.Core.BaseEntity.DomainObjects;
using RssStore.Payment.Business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RssStore.Payment.Business.Entities
{
    public class Transaction : Entity
    {

        public Guid OrderId { get; private set; }
        public Guid PaymentId { get; private set; }
        public decimal Total { get; private set; }
        public TransactionStatus TransactionStatus { get; private set; }

        public Transaction(Guid orderId, Guid paymentId, decimal total)
        {
            OrderId = orderId;
            PaymentId = paymentId;
            Total = total;
        }

        protected Transaction() { }

        //EF Rel
        public virtual Payment Payment { get; set; }

        public void ChangeTransactionStatus(TransactionStatus status)
        {
            TransactionStatus = status;
        }
    }
}
