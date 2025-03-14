namespace Shared.Contracts.Bookings
{
    public record UpdateBookingEvent(
        Guid Id,
        Guid HotelId,
        Guid RoomId,
        DateTime StartDate,
        DateTime EndDate);
}
