using FluentValidation;
using UserManagementService.DTOs;

namespace UserManagementService.Validators
{
    public class AccountVerificationRequestDTOValidator : AbstractValidator<AccountVerificationRequestDTO>
    {
        public AccountVerificationRequestDTOValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(300);
        }
    }
}