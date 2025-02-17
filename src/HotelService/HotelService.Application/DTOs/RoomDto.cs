namespace HotelService.Application.DTOs
{
    public record RoomDto(
        Guid Id,
        Guid HotelId,
        int Capacity,
        int Area,
        int Number,
        int moneyAmount,
        string Currency,
        string ImageUrl,
        List<BookedDatesDto> BookedDates);
}
