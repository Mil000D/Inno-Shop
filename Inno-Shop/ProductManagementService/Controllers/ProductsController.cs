using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagementService.DAL.Context;
using ProductManagementService.DTOs;
using ProductManagementService.Models;
using System.Security.Claims;

namespace ProductManagementService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "User, Admin")]
    public class ProductsController(ProductDbContext context) : ControllerBase
    {
        private readonly ProductDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var roles = User?.FindAll(ClaimTypes.Role).Select(r => r.Value);
            if (roles is not null && roles.Contains("Admin"))
            {
                return await _context.Products.Where(p => p.IsAvailable).ToListAsync();
            }
            else if(int.TryParse(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                return await _context.Products.Where(p => p.CreatorUserId == userId).ToListAsync();
            }
            return BadRequest("User was not found.");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var roles = User?.FindAll(ClaimTypes.Role).Select(r => r.Value);
            Product? product = null;
            if (roles is not null && roles.Contains("Admin"))
            {
                product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && p.IsAvailable);
            }
            else if (int.TryParse(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                product = await _context.Products.FirstOrDefaultAsync(p => p.CreatorUserId == userId && p.Id == id);
            }
            else
            {
                return BadRequest("User was not found.");
            }
            return product is null ? NotFound("Such product was not found.") : Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(ProductDTO productDTO)
        {
            if (int.TryParse(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                var product = new Product
                {
                    Name = productDTO.Name,
                    Description = productDTO.Description,
                    Price = productDTO.Price,
                    IsAvailable = true,
                    CreatorUserId = userId,
                };
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            return BadRequest("User was not found.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductDTO product)
        {
            if(int.TryParse(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                var existingProduct = await _context.Products.FindAsync(id);
                if (existingProduct is null || existingProduct.CreatorUserId != userId)
                {
                    return NotFound("Such product was not found.");
                }

                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                _context.Entry(existingProduct).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            return BadRequest("User was not found.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (int.TryParse(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                var product = await _context.Products.FindAsync(id);
                if (product is null || product.CreatorUserId != userId)
                {
                    return NotFound("Such product was not found.");
                }
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return Ok("Successfully deleted product.");
            }
            return BadRequest("User was not found.");
        }
    }
}
