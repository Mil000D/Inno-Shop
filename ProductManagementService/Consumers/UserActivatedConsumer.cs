using EventBus;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProductManagementService.DAL.Context;

namespace ProductManagementService.Consumers
{
    public class UserActivatedConsumer(ProductDbContext context) : IConsumer<UserActivated>
    {
        private readonly ProductDbContext _context = context;
        public async Task Consume(ConsumeContext<UserActivated> userActivated)
        {
            var products = await _context.Products.Where(p => p.CreatorUserId == userActivated.Message.Id).ToListAsync();
            foreach (var product in products)
            {
                product.IsAvailable = true;
            }

            _context.Products.UpdateRange(products);

            await _context.SaveChangesAsync();
        }
    }
}
