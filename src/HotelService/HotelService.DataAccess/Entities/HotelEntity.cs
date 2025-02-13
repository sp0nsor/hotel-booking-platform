namespace HotelService.DataAccess.Entities
{
    public class HotelEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string PriceCategory { get; set; } = string.Empty;
        public ICollection<RoomEntity> Rooms { get; set; } = [];
    }
}
