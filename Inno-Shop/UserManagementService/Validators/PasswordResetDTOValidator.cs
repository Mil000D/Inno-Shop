using FluentValidation;
using UserManagementService.DTOs;

namespace UserManagementService.Validators
{
    public class PasswordResetDTOValidator : AbstractValidator<PasswordResetDTO>
    {
        public PasswordResetDTOValidator()
        {
            RuleFor(x => x.Token).NotEmpty();
            RuleFor(x => x.NewPassword).NotEmpty().MaximumLength(200);
        }
    }
}