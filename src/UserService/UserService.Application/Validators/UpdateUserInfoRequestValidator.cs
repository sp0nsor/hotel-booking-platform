using FluentValidation;
using UserService.Application.Requests;

namespace UserService.Application.Validators
{
    public class UpdateUserInfoRequestValidator : AbstractValidator<UpdateUserInfoRequest>
    {
        public UpdateUserInfoRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required")
                .Matches(@"^\+375\d{9}$").WithMessage("Invalid phone number format");
        }
    }
}
