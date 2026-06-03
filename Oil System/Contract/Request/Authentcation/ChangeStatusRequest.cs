using FluentValidation;

namespace Oil_System.Contract.Request.Authentcation
{
    public class ChangeStatusRequest
    {
        public string UserId { get; set; }
        public bool IsActive { get; set; } = false;
    }

    public class ChangeStatusRequestValidator : AbstractValidator<ChangeStatusRequest>
    {
        public ChangeStatusRequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.")
                .Must(id => Guid.TryParse(id, out _)).WithMessage("Invalid UserId format.");
        }
    }
}
