using ProductManagementService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductManagementService.DAL.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsByUserIdAsync(int userId);
        Task<IEnumerable<Product>> GetAvailableProductsAsync();
        Task<Product?> GetAvailableProductByIdAsync(int id);
        Task<Product?> GetProductByIdAndUserIdAsync(int id, int userId);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Product product);
    }
}
