using Shop.WebApi.DataAccess;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Shop.WebApi.Controllers
{
    [RoutePrefix("api")]
    public class ProductsController : ApiController
    {
        private readonly IProductTable ProductTable;

        [ImportingConstructor]
        public ProductsController(IProductTable productTable)
        {
            this.ProductTable = productTable;
        }

        [Route("products")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<ProductEntity>))]
        public async Task<IHttpActionResult> GetAllProducts()
        {
            return this.Ok(await this.ProductTable.GetAllProductsAsync());
        }
    }
}