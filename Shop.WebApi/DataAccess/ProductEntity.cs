using Microsoft.WindowsAzure.Storage.Table;
using Shop.WebApi.Model;
using System;

namespace Shop.WebApi.DataAccess
{
    public class ProductEntity : TableEntity
    {
        public string Description { get; set; }

        public int UnitPrice { get; set; }

        public Product ToModel() =>
            new Product
            {
                ProductId = Int32.Parse(this.RowKey),
                Description = this.Description,
                UnitPrice = Convert.ToDecimal(this.UnitPrice) / 100
            };
    }
}