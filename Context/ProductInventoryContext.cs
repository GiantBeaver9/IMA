using Microsoft.EntityFrameworkCore;
using Sample.Models;

namespace Sample.Context
{
    public class ProductInventoryContext : DbContext
    {
        public ProductInventoryContext(DbContextOptions<ProductInventoryContext> options) : base(options)
        {
        }

        public DbSet<ProductInventory> ProductInventory { get; set; }
    }
}
