using CSharpFunctionalExtensions;
using HotelService.Core.ValueObjects;

namespace HotelService.Core.Models
{
    public class Room
    {
        private List<DateRange> _bookedDates = [];

        public Guid Id { get; }
        public Guid HotelId { get; }
        public int Capacity { get; }
        public int Area { get; }
        public int Number {  get; }
        public Image Image { get; }
        public Money Price { get; }
        public IReadOnlyCollection<DateRange> BookedDates => _bookedDates;

        private Room(
            Guid id,
            Guid hotelId,
            int capacity,
            int area, 
            int number,
            Money price,
            Image image,
            List<DateRange>? bookedDates = null)
        {
            Id = id;
            HotelId = hotelId;
            Capacity = capacity;
            Area = area;
            Number = number;
            Price = price;
            Image = image;
            _bookedDates = bookedDates ?? [];
        }

        public static Result<Room> Create(
            Guid id,
            Guid hotelId,
            int capacity,
            int area,
            int number,
            decimal moneyAmount,
            string currency,
            string imageUrl, 
            List<DateRange>? bookedDates = null)
        {
            if (capacity < 0)
                return Result.Failure<Room>("Capacity can not be negative");

            if (area < 0)
                return Result.Failure<Room>("Area can not be negative");

            if (number < 0)
                return Result.Failure<Room>("Room number can not be negative");

            var priceResult = Money.Create(moneyAmount, currency);
            if (priceResult.IsFailure)
                return Result.Failure<Room>(priceResult.Error);

            var imageResult = Image.Create(imageUrl);
            if(imageResult.IsFailure)
                return Result.Failure<Room>(imageResult.Error);

            var room = new Room(
                id, 
                hotelId,
                capacity, 
                area, 
                number,
                priceResult.Value, 
                imageResult.Value,
                bookedDates);

            return Result.Success(room);
        }
    }
}
