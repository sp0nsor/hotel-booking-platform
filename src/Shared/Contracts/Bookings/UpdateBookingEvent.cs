namespace Shared.Contracts.Bookings
{
    public record UpdateBookingEvent(
        Guid HotelId,
        Guid RoomId,
        DateTime StartDate,
        DateTime EndDate);
}
