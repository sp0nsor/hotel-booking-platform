using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace HotelService.Core.ValueObjects
{
    public class PhoneNumber : ValueObject
    {
        private const string pattern = @"^\+375\d{9}$";
        public string Value { get; }

        private PhoneNumber(string value)
        {
            Value = value;
        }

        public static Result<PhoneNumber> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result.Failure<PhoneNumber>("Phone number can not be empty");

            if (!Regex.IsMatch(value, pattern))
                return Result.Failure<PhoneNumber>("Phone number is incorrect");

            return Result.Success(new PhoneNumber(value));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
