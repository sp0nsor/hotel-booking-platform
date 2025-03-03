namespace HotelService.API.Contracts.Hotels
{
    public record CreateHotelRequest(
        string Name,
        string Description,
        string PhoneNumber,
        string Country,
        string City,
        string Street,
        string PriceCategory,
        IFormFile Image);
}
