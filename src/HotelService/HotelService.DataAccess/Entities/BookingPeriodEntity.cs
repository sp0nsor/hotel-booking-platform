namespace HotelService.DataAccess.Entities
{
    public class BookingPeriodEntity
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
