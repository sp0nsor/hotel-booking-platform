namespace Shared.Contracts.Bookings
{
    public record CancelBookingEvent(
        Guid HotelId,
        Guid RoomId,
        DateTime StartDate,
        DateTime EndDate);
}
