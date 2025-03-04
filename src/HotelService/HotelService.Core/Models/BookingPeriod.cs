using CSharpFunctionalExtensions;

namespace HotelService.Core.Models
{
    public class BookingPeriod
    {
        private BookingPeriod(
            Guid id,
            Guid roomId,
            DateTime startDate,
            DateTime endDate)
        {
            Id = id;
            RoomId = roomId;
            StartDate = startDate.ToUniversalTime();
            EndDate = endDate.ToUniversalTime();
        }

        public Guid Id { get; }
        public Guid RoomId { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }

        public static Result<BookingPeriod> Create(
            Guid id,
            Guid roomId,
            DateTime startDate,
            DateTime endDate)
        {
            if (startDate < DateTime.UtcNow)
                return Result.Failure<BookingPeriod>("Start date can not be in past");

            if (startDate > endDate)
                return Result.Failure<BookingPeriod>("Start date can not be after end date");

            var bookedDates = new BookingPeriod(id, roomId, startDate, endDate);

            return Result.Success(bookedDates);
        }

        public bool Overlaps(BookingPeriod other)
        {
            return StartDate < other.EndDate && other.StartDate < EndDate;
        }
    }
}
