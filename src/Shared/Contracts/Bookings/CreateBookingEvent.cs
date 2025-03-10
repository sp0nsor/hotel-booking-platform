namespace Shared.Contracts.Bookings
{
    public record CreateBookingEvent(
        Guid HotelId,
        Guid RoomId,
        DateTime StartDate,
        DateTime EndDate);
}
