using Microsoft.EntityFrameworkCore;
using ProductManagementService.Models;

namespace ProductManagementService.DAL.Context
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
    }
}
