using ProductService.Models;

namespace ProductService.Services
{
    public interface IProductService
    {
        Task<Product?> GetProductByIdAsync(int productId);
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> AddProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int productId);
        Task<int> InsertBulkProductsAsync(IEnumerable<Product> products);
        Task<bool> ProductExistsAsync(int productId);
    }
}
