using EventBus;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProductManagementService.DAL.Context;

namespace ProductManagementService.Consumers
{
    public class UserDeletedConsumer(ProductDbContext context) : IConsumer<UserDeleted>
    {
        private readonly ProductDbContext _context = context;
        public async Task Consume(ConsumeContext<UserDeleted> userDeleted)
        {
            Console.WriteLine("Consuming animal created " + userDeleted.Message.Id);

            var products = await _context.Products.Where(p => p.CreatorUserId == userDeleted.Message.Id).ToListAsync();
            foreach (var product in products)
            {
                product.IsAvailable = false;
            }

            await _context.SaveChangesAsync();
        }
    }
}
