using CSharpFunctionalExtensions;

namespace HotelService.Core.ValueObjects
{
    public class Money : ValueObject
    {
        private static readonly string[] _currencies =
            [
                "USD",
                "EUR",
                "CNY",
                "RUB",
                "BYN",
            ];

        private Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public decimal Amount { get; }
        public string Currency { get; }

        public static Result<Money> Create(decimal amount, string currency)
        {
            if (amount < 0)
                return Result.Failure<Money>("Amount can not be negative");

            if (!_currencies.Any(c => c == currency.Trim().ToUpper()))
                return Result.Failure<Money>("Incorrect currency type");

            var money = new Money(amount, currency);

            return Result.Success(money);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }
    }
}
