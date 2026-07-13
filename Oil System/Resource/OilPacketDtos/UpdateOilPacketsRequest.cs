using FluentValidation;

namespace Oil_System.Resource.OilPacketDtos
{
    public class UpdateOilPacketsRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public double Quantity { get; set; }
        public int Capacity { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalPrice { get; set; }
        public decimal BuyingPrice { get; set; }
        public Guid BrandId { get; set; }
        public Guid CategoryId { get; set; }
    }

    public class UpdateOilPacketsRequestValidator : AbstractValidator<UpdateOilPacketsRequest>
    {
        public UpdateOilPacketsRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Oil packet name is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Oil packet description is required.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero.");
            RuleFor(x => x.Capacity).GreaterThan(0).WithMessage("Capacity must be greater than zero.");
            RuleFor(x => x.Discount).GreaterThanOrEqualTo(0).WithMessage("Discount cannot be negative.");
            RuleFor(x => x.FinalPrice).GreaterThanOrEqualTo(0).WithMessage("Final price cannot be negative.");
            RuleFor(x => x.BuyingPrice).GreaterThanOrEqualTo(0).WithMessage("Buying price cannot be negative.");
            RuleFor(x => x.BrandId).NotEmpty().WithMessage("Brand ID is required.");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category ID is required.");
        }
    }
}
