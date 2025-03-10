namespace BookingService.Infrastructure.Data.Entities
{
    public class BookingEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid HotelId { get; set; }
        public Guid RoomId { get; set; }
        public bool IsOutdated { get; set; } = false;
        public string GuestFirstName { get; set; } = string.Empty;
        public string GuestLastName { get; set; } = string.Empty;
        public string GuestPhoneNumber { get; set; } = string.Empty;
        public string GuestEmail { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
