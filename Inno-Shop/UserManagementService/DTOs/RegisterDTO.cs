using UserManagementService.Enums;
namespace UserManagementService.DTOs
{
    public class RegisterDTO
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public  Role Role { get; set; }
    }
}
