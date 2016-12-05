using Microsoft.ApplicationInsights;
using Microsoft.WindowsAzure.Storage.Table;
using Shop.WebApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.WebApi.DataAccess
{
    public class OrderTable : DataAccessTable, IOrderTable
    {
        public OrderTable() : base("orders") { }

        public async Task<IEnumerable<OrderOutput>> AddOrderLinesAsync(IEnumerable<OrderInput> orderLines)
        {
            var orderTable = await this.GetTableAsync();

            var products = await new ProductTable().GetAllProductsAsync();
            var orderId = Guid.NewGuid();
            var outputLines = new List<OrderOutput>(orderLines.Count());
            foreach (var o in orderLines)
            {
                var product = products.FirstOrDefault(p => p.ProductId == o.ProductId);

                if (product == null)
                {
                    var tc = new TelemetryClient();
                    tc.TrackEvent($"Ignoring unknown product {o.ProductId}");
                    continue;
                }

                outputLines.Add(new OrderOutput
                {
                    OrderId = orderId,
                    ProductId = o.ProductId,
                    Amount = o.Amount,
                    UnitPrice = product.UnitPrice,
                    Rebate = 0,
                    TotalPrice = o.Amount * product.UnitPrice
                });
            }

            if (outputLines.Sum(o => o.TotalPrice) > 1000)
            {
                outputLines.ForEach(o =>
                {
                    o.Rebate = 0.02M;
                    o.TotalPrice = o.TotalPrice * 0.98M;
                });
            }

            var batchOperation = new TableBatchOperation();
            foreach (var o in outputLines)
            {
                batchOperation.Insert(new OrderEntity
                {
                    PartitionKey = o.OrderId.ToString(),
                    RowKey = Guid.NewGuid().ToString(),
                    ProductId = o.ProductId,
                    Amount = Convert.ToInt32(o.Amount * 100),
                    UnitPrice = Convert.ToInt32(o.UnitPrice * 100),
                    Rebate = Convert.ToInt32(o.Rebate * 100),
                    TotalPrice = Convert.ToInt32(o.TotalPrice * 100)
                });
            }

            await orderTable.ExecuteBatchAsync(batchOperation);

            return outputLines;
        }
    }
}