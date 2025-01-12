using UserManagementService.DTOs;
using UserManagementService.Models;

namespace UserManagementService.Services
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginDTO login);
        Task RequestPasswordResetAsync(PasswordResetRequestDTO request);
        Task ResetPasswordAsync(PasswordResetDTO reset);
        Task VerifyAccountAsync(AccountVerificationDTO verification);
    }
}
