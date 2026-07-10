using FluentValidation;

namespace Oil_System.Resource.CategoryDtos
{
    public class CreateCategoryRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
    {
        public CreateCategoryRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Category name is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Category description is required.");
        }
    }
}
