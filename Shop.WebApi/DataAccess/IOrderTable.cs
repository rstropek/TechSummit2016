using Shop.WebApi.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.WebApi.DataAccess
{
    public interface IOrderTable
    {
        Task<IEnumerable<OrderOutput>> AddOrderLinesAsync(IEnumerable<OrderInput> orderLines);
    }
}