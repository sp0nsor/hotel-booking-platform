using FluentValidation;
using UserService.Application.Requests;

namespace UserService.Application.Validators
{
    public class ConfirmUserRequestValidator : AbstractValidator<ConfirmUserRequest>
    {
        public ConfirmUserRequestValidator()
        {
            RuleFor(x => x.ConfirmCode)
                .NotEmpty().WithMessage("Confirm code is required")
                .Matches(@"^\d{6}$").WithMessage("Confirm code is wrong");
        }
    }
}
