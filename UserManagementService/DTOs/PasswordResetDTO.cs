namespace UserManagementService.DTOs
{
    public class PasswordResetDTO
    {
        public required string Token { get; set; }
        public required string NewPassword { get; set; }
    }
}
