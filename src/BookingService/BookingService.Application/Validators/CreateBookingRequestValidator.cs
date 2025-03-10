using BookingService.Application.Requests;
using FluentValidation;

namespace BookingService.Application.Validators
{
    public class CreateBookingRequestValidator : AbstractValidator<CreateBookingRequest>
    {
        public CreateBookingRequestValidator()
        {
            RuleFor(r => r.StartDate)
                .NotEmpty()
                .WithMessage("Start date is required")
                .LessThan(r => r.EndDate)
                .WithMessage("Start date must be before end date");

            RuleFor(r => r.EndDate)
                .NotEmpty()
                .WithMessage("End date is required.")
                .GreaterThan(r => r.StartDate)
                .WithMessage("End date must be after start date");

            RuleFor(r => r.EndDate)
                .Must((request, endDate) => (endDate - request.StartDate).TotalDays >= 1)
                .WithMessage("Booking must be for at least one day");
        }
    }
}
