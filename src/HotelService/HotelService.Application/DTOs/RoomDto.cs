using HotelService.Core.ValueObjects;

namespace HotelService.Application.DTOs
{
    public record RoomDto(
        Guid Id,
        Guid HotelId,
        int Capacity,
        int Area,
        int Number,
        Image Image,
        Money Price,
        List<DateRange> BookedDates);
}
