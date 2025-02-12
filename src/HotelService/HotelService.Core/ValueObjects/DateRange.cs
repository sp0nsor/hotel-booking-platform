using CSharpFunctionalExtensions;

namespace HotelService.Core.ValueObjects
{
    public class DateRange : ValueObject
    {
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }

        private DateRange(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public static Result<DateRange> Create(DateTime startDate, DateTime endDate)
        {
            if (startDate < DateTime.Now)
                return Result.Failure<DateRange>("Start date can not be in past");

            if (startDate > endDate)
                return Result.Failure<DateRange>("Start date can not be after end date");

            var dateRage = new DateRange(startDate, endDate);

            return Result.Success(dateRage);
        }

        public bool Overlaps(DateRange other)
        {
            return StartDate < other.EndDate && other.StartDate < EndDate;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return StartDate;
            yield return EndDate;
        }
    }
}
