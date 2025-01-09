using ProductManagementService.DAL.Repositories;
using ProductManagementService.DTOs;
using ProductManagementService.Models;
using System.Security.Claims;

namespace ProductManagementService.Services
{
    public class ProductService(IProductRepository productRepository) : IProductService
    {
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<IEnumerable<Product>> GetProductsAsync(ClaimsPrincipal user)
        {
            var roles = user?.FindAll(ClaimTypes.Role).Select(r => r.Value);
            if (roles is not null && roles.Contains("Admin"))
            {
                return await _productRepository.GetAvailableProductsAsync();
            }
            else if (int.TryParse(user?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                return await _productRepository.GetProductsByUserIdAsync(userId);
            }
            throw new UnauthorizedAccessException("User was not found.");
        }

        public async Task<Product?> GetProductAsync(int id, ClaimsPrincipal user)
        {
            var roles = user?.FindAll(ClaimTypes.Role).Select(r => r.Value);
            if (roles is not null && roles.Contains("Admin"))
            {
                return await _productRepository.GetAvailableProductByIdAsync(id);
            }
            else if (int.TryParse(user?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                return await _productRepository.GetProductByIdAndUserIdAsync(id, userId);
            }
            throw new UnauthorizedAccessException("User was not found.");
        }

        public async Task<Product> CreateProductAsync(ProductDTO productDTO, ClaimsPrincipal user)
        {
            if (int.TryParse(user?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                var product = new Product
                {
                    Name = productDTO.Name,
                    Description = productDTO.Description,
                    Price = productDTO.Price,
                    IsAvailable = true,
                    CreatorUserId = userId,
                };
                await _productRepository.AddProductAsync(product);
                return product;
            }
            throw new UnauthorizedAccessException("User was not found.");
        }

        public async Task UpdateProductAsync(int id, ProductDTO productDTO, ClaimsPrincipal user)
        {
            if (int.TryParse(user?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                var existingProduct = await _productRepository.GetProductByIdAndUserIdAsync(id, userId)
                    ?? throw new KeyNotFoundException("Such product was not found.");
                existingProduct.Name = productDTO.Name;
                existingProduct.Description = productDTO.Description;
                existingProduct.Price = productDTO.Price;
                await _productRepository.UpdateProductAsync(existingProduct);
            }
            else
            {
                throw new UnauthorizedAccessException("User was not found.");
            }
        }

        public async Task DeleteProductAsync(int id, ClaimsPrincipal user)
        {
            if (int.TryParse(user?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                var product = await _productRepository.GetProductByIdAndUserIdAsync(id, userId) ?? throw new KeyNotFoundException("Such product was not found.");
                await _productRepository.DeleteProductAsync(product);
            }
            else
            {
                throw new UnauthorizedAccessException("User was not found.");
            }
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(ClaimsPrincipal user, string? name, string? description, decimal? minPrice, decimal? maxPrice, bool? isAvailable)
        {
            var roles = user?.FindAll(ClaimTypes.Role).Select(r => r.Value);
            if (roles is not null && roles.Contains("Admin"))
            {
                return await _productRepository.SearchProductsAsync(name, description, minPrice, maxPrice, isAvailable);
            }
            else if (int.TryParse(user?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                return await _productRepository.SearchProductsAsync(name, description, minPrice, maxPrice, isAvailable, userId);
            }
            throw new UnauthorizedAccessException("User was not found.");
        }
    }
}
