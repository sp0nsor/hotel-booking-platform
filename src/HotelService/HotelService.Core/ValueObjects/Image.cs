using CSharpFunctionalExtensions;

namespace HotelService.Core.ValueObjects
{
    public class Image : ValueObject
    {
        private Image(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<Image> Create(string value)
        {
            if (string.IsNullOrEmpty(value))
                return Result.Failure<Image>("Image url can not be null or empty");

            var image = new Image(value);

            return Result.Success(image);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
