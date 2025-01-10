using FluentValidation;
using UserManagementService.DTOs;

namespace UserManagementService.Validators
{
    public class PasswordResetRequestDTOValidator : AbstractValidator<PasswordResetRequestDTO>
    {
        public PasswordResetRequestDTOValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(300);
        }
    }
}