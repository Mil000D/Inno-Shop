using EventBus;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProductManagementService.DAL.Context;

namespace ProductManagementService.Consumers
{
    public class UserDeactivatedConsumer(ProductDbContext context) : IConsumer<UserDeactivated>
    {
        private readonly ProductDbContext _context = context;
        public async Task Consume(ConsumeContext<UserDeactivated> userDeactivated)
        {
            var products = await _context.Products.Where(p => p.CreatorUserId == userDeactivated.Message.Id).ToListAsync();
            foreach (var product in products)
            {
                product.IsAvailable = false;
            }

            _context.Products.UpdateRange(products);

            await _context.SaveChangesAsync();
        }
    }
}