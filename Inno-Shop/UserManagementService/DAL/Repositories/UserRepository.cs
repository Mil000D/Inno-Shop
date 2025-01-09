using Microsoft.EntityFrameworkCore;
using UserManagementService.DAL.Context;
using UserManagementService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserManagementService.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;

        public UserRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsersAsync(int currentUserId)
        {
            return await _context.Users.Where(u => u.Id != currentUserId).ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id, int currentUserId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.Id != currentUserId);
        }

        public async Task<User?> GetActivatedUserByIdAsync(int id, int currentUserId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.Id != currentUserId && u.IsActive);
        }

        public async Task<User?> GetDeactivatedUserByIdAsync(int id, int currentUserId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.Id != currentUserId && !u.IsActive);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}
