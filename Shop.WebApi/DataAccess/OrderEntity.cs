using Microsoft.WindowsAzure.Storage.Table;

namespace Shop.WebApi.DataAccess
{
    public class OrderEntity : TableEntity
    {
        public int ProductId { get; set; }

        public int Amount { get; set; }

        public int UnitPrice { get; set; }

        public int Rebate { get; set; }

        public int TotalPrice { get; set; }
    }
}