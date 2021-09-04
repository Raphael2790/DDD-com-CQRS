using System;
using System.Collections.Generic;
using System.Text;

namespace RssStore.Payment.Business.DTOs
{
    public class Product
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        public decimal Value { get; set; }
    }
}
