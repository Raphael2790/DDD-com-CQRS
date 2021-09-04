using System;

namespace RssStore.Core.DomainObjects.DTOs
{
    public class OrderPayment
    {
        public Guid OrderId { get; set; }
        public Guid ClientId { get; set; }
        public decimal Total { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string CardExpirationDate { get; set; }
        public string CardCvv { get; set; }
    }
}
