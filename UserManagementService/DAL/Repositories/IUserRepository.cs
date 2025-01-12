using UserManagementService.Models;

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
        Task<User?> GetUserByPasswordResetTokenAsync(string token);
        Task<User?> GetUserByAccountVerificationTokenAsync(string token);
        Task SaveChangesAsync();
    }
}