using CSharpFunctionalExtensions;

namespace HotelService.Core.ValueObjects
{
    public class Image : ValueObject
    {
        public string Url { get; }

        private Image(string url)
        {
            Url = url;
        }

        public static Result<Image> Create(string url)
        {
            if (string.IsNullOrEmpty(url))
                return Result.Failure<Image>("Image url can not be null or empty");

            var image = new Image(url);

            return Result.Success(image);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Url;
        }
    }
}
