namespace BookingService.Application.Requests
{
    public record GetBookingsRequest(
        int PageIndex,
        int PageSize,
        string? SearchFirstName,
        string? SearchLastName,
        bool? IsOutDate);
}
