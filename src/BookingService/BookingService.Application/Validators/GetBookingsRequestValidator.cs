using BookingService.Application.Requests;
using FluentValidation;

namespace BookingService.Application.Validators
{
    public class GetBookingsRequestValidator : AbstractValidator<GetBookingsRequest>
    {
        public GetBookingsRequestValidator()
        {
            RuleFor(r => r.PageIndex)
                .GreaterThan(0)
                .WithMessage("PageIndex must be than 0");

            RuleFor(r => r.PageSize)
                .GreaterThan(0)
                .WithMessage("PageSize must be than 0")
                .LessThanOrEqualTo(100)
                .WithMessage("PageSize must not exceed 100");

            When(r => !string.IsNullOrEmpty(r.SearchFirstName), () =>
            {
                RuleFor(r => r.SearchFirstName)
                    .MaximumLength(100)
                    .WithMessage("FirstName must not exceed 100 characters");
            });

            When(r => !string.IsNullOrEmpty(r.SearchLastName), () =>
            {
                RuleFor(r => r.SearchLastName)
                    .MaximumLength(100)
                    .WithMessage("LastName must not exceed 100 characters");
            });

            When(r => r.IsOutDate.HasValue, () =>
            {
                RuleFor(r => r.IsOutDate)
                    .Must(value => value == true || value == false)
                    .WithMessage("IsOutDate must be true or false.");
            });
        }
    }
}
