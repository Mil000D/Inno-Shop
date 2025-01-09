using FluentValidation;
using UserManagementService.DTOs;

namespace UserManagementService.Validators
{
    public class UserDTOValidator : AbstractValidator<UserDTO>
    {
        public UserDTOValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(300);
            RuleFor(x => x.Role).IsInEnum();
            RuleFor(x => x.Password).NotEmpty().MaximumLength(200);
        }
    }
}
