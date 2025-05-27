using Microsoft.AspNetCore.Mvc;
using ProductGrpc.Protos;
using ProductRestApi.Services;

namespace ProductRestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductsGrpcServiceDriver _grpcService;

        public ProductController(ProductsGrpcServiceDriver grpcService)
        {
            _grpcService = grpcService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> GetProduct(int id)
        {
            var product = await _grpcService.GetProductAsync(id);
            return Ok(product);
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductModel>>> GetAll()
        {
            var products = await _grpcService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<ProductModel>> Add(ProductModel model)
        {
            var result = await _grpcService.AddProductAsync(model);
            return CreatedAtAction(nameof(GetProduct), new { id = result.ProductId }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductModel>> Update(int id, ProductModel model)
        {
            model.ProductId = id;
            var updated = await _grpcService.UpdateProductAsync(model);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _grpcService.DeleteProductAsync(id);
            return success ? NoContent() : NotFound();
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> BulkInsert(List<ProductModel> models)
        {
            var (success, count) = await _grpcService.InsertBulkProductAsync(models);
            return success ? Ok(new { count }) : BadRequest("Bulk insert failed");
        }
    }
}
