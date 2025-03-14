namespace BookingService.Application.Requests
{
    public record GetBookingsRequest(
        int PageIndex,
        int PageSize,
        bool? IsOutDate);
}
