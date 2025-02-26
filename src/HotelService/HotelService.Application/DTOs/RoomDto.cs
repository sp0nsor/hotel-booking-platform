namespace HotelService.Application.DTOs
{
    public record RoomDto(
        Guid Id,
        Guid HotelId,
        int Capacity,
        int Area,
        int Number,
        string ImageUrl,
        decimal MoneyAmount,
        string Currency,
        List<BookedDatesDto> BookedDates);
}
