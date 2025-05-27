using Microsoft.EntityFrameworkCore;
using ProductService.DataContext;
using ProductService.Models;

namespace ProductService.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductsContext _context;

        public ProductRepository(ProductsContext context)
        {
            _context = context;
        }

        public async Task<Product?> GetProductByIdAsync(int productId) =>
            await _context.Product.FindAsync(productId);

        public async Task<List<Product>> GetAllProductsAsync() =>
            await _context.Product.ToListAsync();

        public async Task<Product> AddProductAsync(Product product)
        {
            _context.Product.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await _context.Product.FindAsync(productId);
            if (product == null) return false;

            _context.Product.Remove(product);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> InsertBulkProductsAsync(IEnumerable<Product> products)
        {
            _context.Product.AddRange(products);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> ProductExistsAsync(int productId) =>
            await _context.Product.AnyAsync(p => p.ProductId == productId);
    }
}
