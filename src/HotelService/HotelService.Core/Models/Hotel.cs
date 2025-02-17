using CSharpFunctionalExtensions;
using HotelService.Core.ValueObjects;

namespace HotelService.Core.Models
{
    public class Hotel
    {
        private List<Room> _rooms = [];

        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }
        public Image Image { get; }
        public PhoneNumber PhoneNumber { get; }
        public Address Address { get; }
        public PriceCategory Category { get; }
        public IReadOnlyCollection<Room>? Rooms => _rooms;

        private Hotel(
            Guid id,
            string name,
            string description,
            PhoneNumber phoneNumber,
            Address address,
            PriceCategory category,
            Image image,
            List<Room>? rooms = null)
        {
            Id = id;
            Name = name;
            Description = description;
            PhoneNumber = phoneNumber;
            Address = address;
            Category = category;
            Image = image;
            _rooms = rooms ?? [];
        }

        public static Result<Hotel> Create(
            Guid id,
            string name,
            string description,
            string phoneNumber,
            string country,
            string city,
            string street,
            string category,
            string imageUrl,
            List<Room>? rooms = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Hotel>("Name can not be null or empty");

            if (string.IsNullOrWhiteSpace(description))
                return Result.Failure<Hotel>("Description can not be null or empty");

            var numberResult = PhoneNumber.Create(phoneNumber);
            if (numberResult.IsFailure)
                return Result.Failure<Hotel>(numberResult.Error);

            var addressResult = Address.Create(country, city, street);
            if (addressResult.IsFailure)
                return Result.Failure<Hotel>(addressResult.Error);

            var catagoryResult = PriceCategory.Create(category);
            if (catagoryResult.IsFailure)
                return Result.Failure<Hotel>(catagoryResult.Error);

            var imageResult = Image.Create(imageUrl);
            if (imageResult.IsFailure)
                return Result.Failure <Hotel>(imageResult.Error);

            var hotel = new Hotel(
                id,
                name,
                description,
                numberResult.Value,
                addressResult.Value,
                catagoryResult.Value,
                imageResult.Value,
                rooms);

            return Result.Success(hotel);
        }

        public Result AddRoom(Room room)
        {
            if (room is null)
                return Result.Failure("Room can not be null");

            if (_rooms.Any(r => r.Number == room.Number))
                return Result.Failure("Room with the same number alredy axists");

            _rooms.Add(room);
            
            return Result.Success();
        }

        public Result RemoveRoom(int roomNumber)
        {
            var room  = _rooms.FirstOrDefault(r => r.Number == roomNumber);
            if (room is null)
                return Result.Failure("Room not found");

            _rooms.Remove(room);

            return Result.Success();
        }
    }
}
