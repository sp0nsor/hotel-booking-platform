namespace HotelService.Application.DTOs
{
    public record HotelDto(
        Guid Id,
        string Name,
        string Description,
        string PhoneNumber,
        string Country,
        string City,
        string Street,
        string Category,
        string ImageUrl,
        List<RoomDto> Rooms);
}
