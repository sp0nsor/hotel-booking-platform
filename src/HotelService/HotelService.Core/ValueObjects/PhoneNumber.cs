using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace HotelService.Core.ValueObjects
{
    public class PhoneNumber : ValueObject
    {
        private const string pattern = @"^\+375\d{9}$";
        public string Number { get; }

        private PhoneNumber(string number)
        {
            Number = number;
        }

        public static Result<PhoneNumber> Create(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                return Result.Failure<PhoneNumber>("Phone number can not be empty");

            if (!Regex.IsMatch(number, pattern))
                return Result.Failure<PhoneNumber>("Phone number is incorrect");

            return Result.Success(new PhoneNumber(number));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Number;
        }
    }
}
