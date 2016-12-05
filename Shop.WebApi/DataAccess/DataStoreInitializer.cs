using Microsoft.WindowsAzure.Storage;
using Shop.WebApi.Controllers;
using System.Threading.Tasks;

namespace Shop.WebApi.DataAccess
{
    public class DataStoreInitializer
    {
        public async Task InitializeDataStoreAsync()
        {
            var storageAccount = CloudStorageAccount.Parse(
                await SecretsManager.GetTableStorageConnectionStringAsync());
            var tableClient = storageAccount.CreateCloudTableClient();

            await tableClient.GetTableReference("products").CreateIfNotExistsAsync();
            await tableClient.GetTableReference("orders").CreateIfNotExistsAsync();
        }
    }
}