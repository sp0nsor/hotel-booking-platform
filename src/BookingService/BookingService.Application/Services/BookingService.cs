using AutoMapper;
using BookingService.Application.DTOs;
using BookingService.Application.Interfaces;
using BookingService.Application.Requests;
using BookingService.Infrastructure.Data.Entities;
using BookingService.Infrastructure.Data.Specifications;
using BookingService.Infrastructure.Interfaces.Data;
using CSharpFunctionalExtensions;
using FluentValidation;

namespace BookingService.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IRepository<BookingEntity> _bookingRepository;
        private readonly IValidator<BookingDataRequest> _bookingDataRequestValidator;
        private readonly IValidator<GetBookingsRequest> _getBookingRequestValidator;
        private readonly IMapper _mapper;

        public BookingService(
            IMapper mapper,
            IRepository<BookingEntity> bookingRepository,
            IValidator<BookingDataRequest> dataRequestValidator,
            IValidator<GetBookingsRequest> getBookingRequestValidator)
        {
            _mapper = mapper;
            _bookingRepository = bookingRepository;
            _bookingDataRequestValidator = dataRequestValidator;
            _getBookingRequestValidator = getBookingRequestValidator;
        }

        public async Task<Result> CreateBookingAsync(
            BookingDataRequest createBookingRequest,
            CancellationToken cancellationToken)
        {
            var validationResult = _bookingDataRequestValidator.Validate(createBookingRequest);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return Result.Failure(string.Join("; ", errors));
            }

            var bookingEntity = _mapper.Map<BookingEntity>(createBookingRequest);
            bookingEntity.Id = Guid.NewGuid();

            await _bookingRepository.CreateAsync(bookingEntity, cancellationToken);

            return Result.Success();
        }

        public async Task<Result<PaginatedResult<BookingDto>>> GetHotelBookingsAsync(
            Guid hotelId,
            GetBookingsRequest getBookingsRequest,
            CancellationToken cancellationToken)
        {
            var validationResult = _getBookingRequestValidator.Validate(getBookingsRequest);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return Result.Failure<PaginatedResult<BookingDto>>(string.Join("; ", errors));
            }

            var hotelBookingsSpecification = new HotelBookingsSpecification(
                hotelId,
                getBookingsRequest.IsOutDate,
                getBookingsRequest.SearchFirstName,
                getBookingsRequest.SearchLastName);

            var (bookingEntities, totalPages) = await _bookingRepository.GetAsync(
                hotelBookingsSpecification,
                getBookingsRequest.PageIndex,
                getBookingsRequest.PageSize,
                cancellationToken);

            return new PaginatedResult<BookingDto>
            {
                Items = _mapper.Map<List<BookingDto>>(bookingEntities),
                CurrentPage = getBookingsRequest.PageIndex,
                PageSize = getBookingsRequest.PageSize,
                TotalPages = totalPages
            };
        }

        public async Task<Result> DeleteBookingAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetByIdAsync(
                id,
                cancellationToken);

            if (booking is null)
                return Result.Failure("Booking not found");

            await _bookingRepository.DeleteAsync(
                booking,
                cancellationToken);

            return Result.Success();
        }

        public async Task<Result> CancelBookingAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetByIdAsync(
                id,
                cancellationToken);

            if (booking is null)
                return Result.Failure("Booking not found");

            booking.IsOutdated = true;

            await _bookingRepository.UpdateAsync(
                booking,
                cancellationToken);

            return Result.Success();
        }

        public async Task<Result> UpdateBookingAsync(
            Guid id,
            BookingDataRequest updateBookingRequest,
            CancellationToken cancellationToken)
        {
            var validationResult = _bookingDataRequestValidator.Validate(updateBookingRequest);

            if(!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return Result.Failure(string.Join("; ", errors));
            }

            var oldBooking = _bookingRepository.GetByIdAsync(
                id,
                cancellationToken);

            if (oldBooking is null)
                return Result.Failure("Booking not found");

            var updatedBooking = _mapper.Map<BookingEntity>(updateBookingRequest);
            updatedBooking.Id = id;

            await _bookingRepository.UpdateAsync(
                updatedBooking,
                cancellationToken);

            return Result.Success();
        }
    }
}
