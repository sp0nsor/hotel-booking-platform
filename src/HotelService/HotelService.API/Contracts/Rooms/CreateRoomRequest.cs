namespace HotelService.API.Contracts.Rooms
{
    public record CreateRoomRequest(
        int Capacity,
        int Area,
        int Number,
        decimal MoneyAmount,
        string Currency,
        IFormFile Image);
}
