namespace HotelService.DataAccess.Entities
{
    public class RoomEntity
    {
        public Guid Id { get; set; }
        public Guid HotelId { get; set; }
        public int Capacity { get; set; }
        public int Area { get; set; }
        public int Number {  get; set; }
        public decimal MoneyAmount { get; set; }
        public string Currency {  get; set; } = string.Empty;
        public ICollection<BookedDateEntity> BookedDateEntities { get; set; } = [];
    }
}