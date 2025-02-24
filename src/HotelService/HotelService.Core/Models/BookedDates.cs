using CSharpFunctionalExtensions;

namespace HotelService.Core.Models
{
    public class BookedDates
    {
        public Guid Id { get; }
        public Guid HotelId { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }

        private BookedDates(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate.ToUniversalTime();
            EndDate = endDate.ToUniversalTime();
        }

        public static Result<BookedDates> Create(DateTime startDate, DateTime endDate)
        {
            if (startDate < DateTime.Now)
                return Result.Failure<BookedDates>("Start date can not be in past");

            if (startDate > endDate)
                return Result.Failure<BookedDates>("Start date can not be after end date");

            var bookedDates = new BookedDates(startDate, endDate);

            return Result.Success(bookedDates);
        }

        public bool Overlaps(BookedDates other)
        {
            return StartDate < other.EndDate && other.StartDate < EndDate;
        }
    }
}
