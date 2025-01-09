using UserManagementService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserManagementService.DAL.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync(int currentUserId);
        Task<User?> GetUserByIdAsync(int id, int currentUserId);
        Task<User?> GetActivatedUserByIdAsync(int id, int currentUserId);
        Task<User?> GetDeactivatedUserByIdAsync(int id, int currentUserId);
        Task<User?> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
        Task<User?> GetUserByIdAsync(int id);
    }
}
