using UserManagementService.Models;

namespace UserManagementService.TokenHandlers
{
    public interface ITokenHandler
    {
        string GenerateGuidBasedToken();
        string GenerateJwtToken(User user);
    }
}
