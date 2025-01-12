using UserManagementService.Enums;

namespace UserManagementService.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required Role Role { get; set; }
        public required string PasswordHash { get; set; }
        public required bool IsActive { get; set; }
        public string? PasswordResetToken { get; set; }
        public required bool IsVerified { get; set; }
        public string? AccountVerificationToken { get; set; }
    }
}