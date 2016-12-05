using Microsoft.Azure.KeyVault;
using Shop.WebApi.AzureUtilities;
using System.Threading.Tasks;

namespace Shop.WebApi.DataAccess
{
    public static class SecretsManager
    {
        private static string TableStorageConnectionString { get; set; }

        public static async Task<string> GetTableStorageConnectionStringAsync()
        {
            if (TableStorageConnectionString == null)
            {
                TableStorageConnectionString = await RefreshSecretsInternalAsync();
            }

            return TableStorageConnectionString;
        }

        private static async Task<string> RefreshSecretsInternalAsync()
        {
            var kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(ActiveDirectoryAccessTokenHelper.GetToken));
            var tableStorageConnectionStringSecret = await kv.GetSecretAsync("https://technical-summit.vault.azure.net/", "SAS-TechnicalSummit-TableStorage");
            return tableStorageConnectionStringSecret.Value;
        }
    }
}