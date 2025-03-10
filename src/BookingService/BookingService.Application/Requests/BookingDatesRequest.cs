namespace BookingService.Application.Requests
{
    public record BookingDatesRequest(
        DateTime StartDate,
        DateTime EndDate);
}
