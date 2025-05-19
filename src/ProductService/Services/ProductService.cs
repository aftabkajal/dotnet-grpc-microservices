using ProductService.Models;
using ProductService.Repositories;

namespace ProductService.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public Task<Product?> GetProductByIdAsync(int productId) =>
            _repository.GetProductByIdAsync(productId);

        public Task<List<Product>> GetAllProductsAsync() =>
            _repository.GetAllProductsAsync();

        public Task<Product> AddProductAsync(Product product) =>
            _repository.AddProductAsync(product);

        public Task<Product> UpdateProductAsync(Product product) =>
            _repository.UpdateProductAsync(product);

        public Task<bool> DeleteProductAsync(int productId) =>
            _repository.DeleteProductAsync(productId);

        public Task<int> InsertBulkProductsAsync(IEnumerable<Product> products) =>
            _repository.InsertBulkProductsAsync(products);

        public Task<bool> ProductExistsAsync(int productId) =>
            _repository.ProductExistsAsync(productId);
    }
}
