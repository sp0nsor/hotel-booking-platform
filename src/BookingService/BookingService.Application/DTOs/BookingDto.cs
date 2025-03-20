namespace BookingService.Application.DTOs
{
    public record BookingDto(
        Guid Id,
        Guid HotelId,
        string HotelName,
        string Country,
        string City, 
        string Street,
        Guid RoomId,
        int RoomNumber,
        string Currency,
        string GuestFirstName,
        string GuestLastName,
        string GuestPhoneNumber,
        string GuestEmail,
        bool IsOutdated,
        DateTime StartDate,
        DateTime EndDate,
        decimal TotalPrice);
}
