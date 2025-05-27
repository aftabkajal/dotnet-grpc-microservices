using Microsoft.EntityFrameworkCore;
using ProductService.Models;

namespace ProductService.DataContext
{
    public class ProductsContext : DbContext
    {
        public ProductsContext(DbContextOptions<ProductsContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Product { get; set; }
    }
}
