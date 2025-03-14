namespace Shared.Contracts.Bookings
{
    public record CreateBookingEvent(
        Guid Id,
        Guid HotelId,
        Guid RoomId,
        DateTime StartDate,
        DateTime EndDate);
}
