using ProductManagementService.DTOs;
using ProductManagementService.Models;
using System.Security.Claims;

namespace ProductManagementService.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync(ClaimsPrincipal user);
        Task<Product?> GetProductAsync(int id, ClaimsPrincipal user);
        Task<Product> CreateProductAsync(ProductDTO productDTO, ClaimsPrincipal user);
        Task UpdateProductAsync(int id, ProductDTO productDTO, ClaimsPrincipal user);
        Task DeleteProductAsync(int id, ClaimsPrincipal user);
        Task<IEnumerable<Product>> SearchProductsAsync(ClaimsPrincipal user, string? name, string? description, decimal? minPrice, decimal? maxPrice, bool? isAvailable);
    }
}
