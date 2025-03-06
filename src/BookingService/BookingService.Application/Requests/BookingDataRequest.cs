namespace BookingService.Application.Requests
{
    public record BookingDataRequest(
        Guid HotelId,
        Guid RoomId,
        string GuestFirstName,
        string GuestLastName,
        string GuestPhoneNumber,
        string GuestEmail,
        DateTime StartDate,
        DateTime EndDate);
}
