namespace Shared.Contracts.Bookings
{
    public record CancelBookingEvent(
        Guid Id,
        Guid RoomId);
}
