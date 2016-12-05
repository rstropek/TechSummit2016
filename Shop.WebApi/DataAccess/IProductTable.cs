using Shop.WebApi.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.WebApi.DataAccess
{
    public interface IProductTable
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
    }
}