namespace BookingService.Application.Requests
{
    public record CreateBookingRequest(
        DateTime StartDate,
        DateTime EndDate);
}
