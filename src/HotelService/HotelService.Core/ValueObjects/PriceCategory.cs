
using CSharpFunctionalExtensions;

namespace HotelService.Core.ValueObjects
{
    public class PriceCategory : ValueObject
    {
        public static readonly PriceCategory Low = new(nameof(Low));
        public static readonly PriceCategory High = new(nameof(High));
        public static readonly PriceCategory Medium = new(nameof(Medium));

        private static readonly PriceCategory[] _all = { Low, High, Medium };

        public string Value { get; }

        private PriceCategory(string value)
        {
            Value = value;
        }

        public static Result<PriceCategory> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result.Failure<PriceCategory>("Price category can not be null");

            var category = value.Trim().ToLower();

            if (!_all.Any(c => c.Value.ToLower() == category))
                return Result.Failure<PriceCategory>("Invalid category type");

            return new PriceCategory(category);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
