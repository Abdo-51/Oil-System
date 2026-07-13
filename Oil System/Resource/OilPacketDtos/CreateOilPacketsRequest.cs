using FluentValidation;

namespace Oil_System.Resource.OilPacketDtos
{
    public class CreateOilPacketsRequest
    {
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

    public class CreateOilPacketsRequestValidator : AbstractValidator<CreateOilPacketsRequest>
    {
        public CreateOilPacketsRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0.");
            RuleFor(x => x.Capacity).GreaterThan(0).WithMessage("Capacity must be greater than 0.");
            RuleFor(x => x.Discount).GreaterThanOrEqualTo(0).WithMessage("Discount must be greater than or equal to 0.");
            RuleFor(x => x.FinalPrice).GreaterThanOrEqualTo(0).WithMessage("Final Price must be greater than or equal to 0.");
            RuleFor(x => x.BuyingPrice).GreaterThan(0).WithMessage("Buying Price must be greater than 0.");
            RuleFor(x => x.BrandId).NotEmpty().WithMessage("BrandId is required.");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("CategoryId is required.");
        }
    }
}
