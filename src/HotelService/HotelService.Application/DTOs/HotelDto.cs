namespace HotelService.Application.DTOs
{
    public record HotelDto(
        Guid Id,
        string Name,
        string Description,
        string ImageUrl,
        string Number,
        string Country,
        string City,
        string Street,
        string PriceCategory,
        List<RoomDto> Rooms);
}
