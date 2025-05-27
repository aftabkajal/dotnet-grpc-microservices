using ProductService.Models;

namespace ProductService.Repositories
{
    public interface IProductRepository
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
