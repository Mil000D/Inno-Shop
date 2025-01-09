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

        public async Task<Product?> GetAvailableProductByIdAsync(int id)
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

        public async Task<IEnumerable<Product>> SearchProductsAsync(string? name, string? description, decimal? minPrice, decimal? maxPrice, bool? isAvailable, int? userId = null)
        {
            var query = _context.Products.AsQueryable();

            if (userId.HasValue)
            {
                query = query.Where(p => p.CreatorUserId == userId.Value);
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(description))
            {
                query = query.Where(p => p.Description.Contains(description));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            if (isAvailable.HasValue)
            {
                query = query.Where(p => p.IsAvailable == isAvailable.Value);
            }

            return await query.ToListAsync();
        }
    }
}