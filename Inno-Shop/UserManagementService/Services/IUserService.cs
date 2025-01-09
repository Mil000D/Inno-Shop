using UserManagementService.DTOs;
using UserManagementService.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace UserManagementService.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersAsync(ClaimsPrincipal user);
        Task<User?> GetUserAsync(int id, ClaimsPrincipal user);
        Task<User> CreateUserAsync(UserDTO register);
        Task UpdateUserAsync(int id, UserDTO user, ClaimsPrincipal userPrincipal);
        Task DeleteUserAsync(int id, ClaimsPrincipal user);
        Task DeactivateUserAsync(int id, ClaimsPrincipal user);
        Task ActivateUserAsync(int id, ClaimsPrincipal user);
    }
}
