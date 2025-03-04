using CSharpFunctionalExtensions;

namespace HotelService.Core.ValueObjects
{
    public class Category : ValueObject
    {
        private Category(string value)
        {
            Value = value;
        }

        public static readonly Category Low = new(nameof(Low));
        public static readonly Category High = new(nameof(High));
        public static readonly Category Medium = new(nameof(Medium));

        private static readonly Category[] existingCategories = { Low, High, Medium };

        public string Value { get; }

        public static Result<Category> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result.Failure<Category>("Price category can not be null");

            var category = value.Trim().ToLower();

            if (!existingCategories.Any(c => c.Value.ToLower() == category))
                return Result.Failure<Category>("Invalid category type");

            return new Category(category);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
