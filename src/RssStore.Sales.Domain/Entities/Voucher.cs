using RssStore.Core.BaseEntity.DomainObjects;
using RssStore.Sales.Domain.Enums;
using System;
using System.Collections.Generic;

namespace RssStore.Sales.Domain.Entities
{
    public class Voucher : Entity 
    {
        public string Code { get; private set; }
        public decimal? Percentual { get; private set; }
        public decimal? DiscountValue { get; private set; }
        public int Amount { get; private set; }
        public VoucherDiscountType VoucherDiscountType { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime ApplyedAt { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public bool Active { get; private set; }
        public bool Applyed { get; private set; }

        //EF Relations
        public virtual ICollection<Order> Orders { get; set; }
    }
}
