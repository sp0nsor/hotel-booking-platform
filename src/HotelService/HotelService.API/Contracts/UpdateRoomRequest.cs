namespace HotelService.API.Contracts
{
    public record UpdateRoomRequest(
        int Area,
        int Number,
        int Capacity,
        decimal MoneyAmount,
        string Currency,
        IFormFile Image);
}
