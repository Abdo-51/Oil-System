using FluentValidation;

namespace Oil_System.Resource.CategoryDtos
{
    public class UpdateCategoryRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
    {
        public UpdateCategoryRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Category ID is required.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Category name is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Category description is required.");
        }
    }
}
