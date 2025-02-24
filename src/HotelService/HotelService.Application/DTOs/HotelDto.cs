using HotelService.Core.ValueObjects;

namespace HotelService.Application.DTOs
{
    public record HotelDto(
        Guid Id,
        string Name,
        string Description,
        Image Image,
        PhoneNumber PhoneNumber,
        Address Address,
        Category Category,
        List<RoomDto> Rooms);
}
