using System;

namespace Shop.WebApi.Model
{
    public class OrderOutput : OrderInput
    {
        public Guid OrderId { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Rebate { get; set; }

        public decimal TotalPrice { get; set; }
    }
}