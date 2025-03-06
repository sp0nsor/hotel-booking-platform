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
        DateTime StartDate,
        DateTime EndDate,
        decimal TotalPrice);
}
