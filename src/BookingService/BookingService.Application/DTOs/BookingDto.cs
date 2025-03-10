namespace BookingService.Application.DTOs
{
    public record BookingDto(
        Guid Id,
        Guid HotelId,
        Guid RoomId,
        string GuestFirstName,
        string GuestLastName,
        string GuestPhoneNumber,
        string GuestEmail,
        bool IsOutdated,
        DateTime StartDate,
        DateTime EndDate);
}
