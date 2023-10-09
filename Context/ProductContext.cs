using Microsoft.EntityFrameworkCore;
using Sample.Models; 

namespace Sample.Context
{
    public class ProductContext :DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
