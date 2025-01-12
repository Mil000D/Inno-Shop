using FluentValidation;
using UserManagementService.DTOs;

namespace UserManagementService.Validators
{
    public class LoginDTOValidator : AbstractValidator<LoginDTO>
    {
        public LoginDTOValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(300);
            RuleFor(x => x.Password).NotEmpty().MaximumLength(200);
        }
    }
}