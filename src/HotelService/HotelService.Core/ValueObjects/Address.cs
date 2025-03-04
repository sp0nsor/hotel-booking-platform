using CSharpFunctionalExtensions;

namespace HotelService.Core.ValueObjects
{
    public class Address : ValueObject
    {
        private Address(
            string country,
            string city,
            string street)
        {
            Country = country;
            City = city;
            Street = street;
        }

        public string Country { get; }
        public string City { get; }
        public string Street { get; }

        public static Result<Address> Create(
            string country,
            string city,
            string street)
        {
            if (string.IsNullOrEmpty(country))
                return Result.Failure<Address>("Country can not be empty");

            if (string.IsNullOrEmpty(city))
                return Result.Failure<Address>("City can not be empty");

            if (string.IsNullOrEmpty(street))
                return Result.Failure<Address>("Street can not be empty");

            return Result.Success(new Address(country, city, street));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Country;
            yield return City;
            yield return Street;
        }
    }
}
