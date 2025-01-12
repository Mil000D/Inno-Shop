using FluentValidation;
using UserManagementService.DTOs;

namespace UserManagementService.Validators
{
    public class AccountVerificationDTOValidator : AbstractValidator<AccountVerificationDTO>
    {
        public AccountVerificationDTOValidator()
        {
            RuleFor(x => x.Token).NotEmpty();
        }
    }
}