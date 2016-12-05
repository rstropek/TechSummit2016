using Microsoft.WindowsAzure.Storage.Table;
using Shop.WebApi.AzureUtilities;
using Shop.WebApi.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.WebApi.DataAccess
{
    public class ProductTable : DataAccessTable, IProductTable
    {
        public ProductTable() : base("products") { }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var productTable = await this.GetTableAsync();
            var products = await productTable.ExecuteQueryAsync(
                new TableQuery<ProductEntity>());
            return products.Select(item => item.ToModel());
        }
    }
}