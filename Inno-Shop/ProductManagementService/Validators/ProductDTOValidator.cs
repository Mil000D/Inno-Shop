using FluentValidation;
using ProductManagementService.DTOs;

namespace ProductManagementService.Validators
{
    public class ProductDTOValidator : AbstractValidator<ProductDTO>
    {
        public ProductDTOValidator()
        {
            RuleFor(productDTO => productDTO.Name).NotEmpty().MaximumLength(100);
            RuleFor(productDTO => productDTO.Description).NotEmpty().MaximumLength(1000);
            RuleFor(productDTO => productDTO.Price).NotEmpty().GreaterThan(0);
        }
    }
}
