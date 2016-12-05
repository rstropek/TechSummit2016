using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Shop.WebApi.Controllers;
using System.Threading.Tasks;

namespace Shop.WebApi.DataAccess
{
    public abstract class DataAccessTable
    {
        protected DataAccessTable(string tableName)
        {
            this.TableName = tableName;
        }

        protected async Task<CloudTableClient> GetTableClientAsync()
        {
            var storageAccount = CloudStorageAccount.Parse(
                await SecretsManager.GetTableStorageConnectionStringAsync());
            return storageAccount.CreateCloudTableClient();
        }

        protected async Task<CloudTable> GetTableAsync() => 
            (await GetTableClientAsync()).GetTableReference(this.TableName);

        protected string TableName { get; }
    }
}