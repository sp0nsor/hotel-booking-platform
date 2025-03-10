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
        }
    }
}
