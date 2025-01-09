using Microsoft.EntityFrameworkCore;
using ProductManagementService.DAL.Context;
using ProductManagementService.Models;

namespace ProductManagementService.DAL.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;

        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProductsByUserIdAsync(int userId)
        {
            return await _context.Products.Where(p => p.CreatorUserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAvailableProductsAsync()
        {
            return await _context.Products.Where(p => p.IsAvailable).ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id && p.IsAvailable);
        }

        public async Task<Product?> GetProductByIdAndUserIdAsync(int id, int userId)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.CreatorUserId == userId && p.Id == id);
        }

        public async Task AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}