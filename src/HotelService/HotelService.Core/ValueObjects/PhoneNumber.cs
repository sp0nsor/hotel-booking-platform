using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace HotelService.Core.ValueObjects
{
    public class PhoneNumber : ValueObject
    {
        private const string _pattern = @"^\+375\d{9}$";

        private PhoneNumber(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<PhoneNumber> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result.Failure<PhoneNumber>("Phone number can not be empty");

            if (!Regex.IsMatch(value, _pattern))
                return Result.Failure<PhoneNumber>("Phone number is incorrect");

            return Result.Success(new PhoneNumber(value));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
