namespace HotelService.API.Contracts
{
    public record CreateRoomRequest(
        int Capacity,
        int Area,
        int Number,
        decimal MoneyAmount,
        string Currency,
        IFormFile Image);
}
