using FluentValidation.Results;
using RssStore.Core.BaseEntity.DomainObjects;
using RssStore.Sales.Domain.EntityValidations;
using RssStore.Sales.Domain.Enums;
using System;
using System.Collections.Generic;

namespace RssStore.Sales.Domain.Entities
{
    public class Voucher : Entity 
    {
        public Voucher(string code, decimal? percentual, decimal? discountValue, int amount, VoucherDiscountType voucherDiscountType, DateTime expirationDate, bool active, bool applyed)
        {
            Code = code;
            Percentual = percentual;
            DiscountValue = discountValue;
            Amount = amount;
            VoucherDiscountType = voucherDiscountType;
            CreatedAt = DateTime.Now;
            ExpirationDate = expirationDate;
            Active = active;
            Applyed = applyed;
        }

        protected Voucher() { }

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

        public ValidationResult IsValidToApply()
        {
            return new ApplyableVoucherValidation().Validate(this);
        }
    }
}
