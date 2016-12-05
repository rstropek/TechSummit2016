using Shop.WebApi.DataAccess;
using Shop.WebApi.Model;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Shop.WebApi.Controllers
{
    [RoutePrefix("api")]
    public class OrdersController : ApiController
    {
        private readonly IOrderTable OrderTable;
        private readonly IProductTable ProductTable;

        [ImportingConstructor]
        public OrdersController(IProductTable productTable, IOrderTable orderTable)
        {
            this.ProductTable = productTable;
            this.OrderTable = orderTable;
        }

        [Route("orders")]
        [HttpPost]
        [ResponseType(typeof(IEnumerable<OrderOutput>))]
        public async Task<IHttpActionResult> AddOrder([FromBody] IEnumerable<OrderInput> orderLines)
        {
            if (!orderLines.Any() || orderLines.Count() > 10)
            {
                return this.BadRequest("Order must consist of >= 1 and <= 10 items.");
            }

            return Created("dummy", await this.OrderTable.AddOrderLinesAsync(orderLines));
        }
    }
}