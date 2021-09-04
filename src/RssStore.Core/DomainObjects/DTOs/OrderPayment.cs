using System;

namespace RssStore.Core.DomainObjects.DTOs
{
    public class OrderPayment
    {
        public Guid OrderId { get; set; }
        public Guid ClientId { get; set; }
        public decimal Total { get; set; }
        public string CardName { get; private set; }
        public string CardNumber { get; private set; }
        public string CardExpirationDate { get; private set; }
        public string CardCvv { get; private set; }
    }
}
