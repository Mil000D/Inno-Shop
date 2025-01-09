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
            await _context.Products
                .Where(p => p.CreatorUserId == userDeleted.Message.Id)
                .ExecuteDeleteAsync();
        }
    }
}
