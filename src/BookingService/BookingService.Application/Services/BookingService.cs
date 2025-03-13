using AutoMapper;
using BookingService.Application.DTOs;
using BookingService.Application.Interfaces;
using BookingService.Application.Requests;
using BookingService.Infrastructure.Data.Entities;
using BookingService.Infrastructure.Data.Specifications;
using BookingService.Infrastructure.Interfaces.Data;
using BookingService.Infrastructure.Interfaces.MessageBroker;
using CSharpFunctionalExtensions;
using FluentValidation;
using Shared.Contracts.Bookings;

namespace BookingService.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IMapper _mapper;
        private readonly IEventBus _eventBus;
        private readonly IRepository<BookingEntity> _bookingRepository;
        private readonly IValidator<GetBookingsRequest> _getBookingRequestValidator;
        private readonly IValidator<CreateBookingRequest> _createBookingRequestValidator;

        public BookingService(
            IMapper mapper,
            IEventBus eventBus,
            IRepository<BookingEntity> bookingRepository,
            IValidator<CreateBookingRequest> dataRequestValidator,
            IValidator<GetBookingsRequest> getBookingRequestValidator)
        {
            _mapper = mapper;
            _eventBus = eventBus;
            _bookingRepository = bookingRepository;
            _createBookingRequestValidator = dataRequestValidator;
            _getBookingRequestValidator = getBookingRequestValidator;
        }

        public async Task<Result> CreateBookingAsync(
            Guid hotelId,
            Guid roomId,
            CreateBookingRequest bookingRequest,
            CancellationToken cancellationToken)
        {
            // gRPC logic
            Guid userId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");
            string guestFirstName = "GuestFirstName";
            string guestLastName = "GuestLastName";
            string guestPhoneNumber = "GuestPhoneNumber";
            string guestEmail = "mazie.zemlak@ethereal.email";

            var validationResult = await _createBookingRequestValidator.ValidateAsync(
                bookingRequest,
                cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return Result.Failure(string.Join("; ", errors));
            }

            if(await HasBookingConflictAsync(
                hotelId, 
                roomId,
                bookingRequest.StartDate, 
                bookingRequest.EndDate))
            {
                return Result.Failure("Date conflict");
            }

            var bookingEntity = new BookingEntity
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                HotelId = hotelId,
                RoomId = roomId,
                GuestFirstName = guestFirstName,
                GuestLastName = guestLastName,
                GuestPhoneNumber = guestPhoneNumber,
                GuestEmail = guestEmail,
                StartDate = bookingRequest.StartDate,
                EndDate = bookingRequest.EndDate
            };

            await _bookingRepository.CreateAsync(
                bookingEntity, 
                cancellationToken);

            var createBookingEvent = _mapper.Map<CreateBookingEvent>(bookingEntity);

            await _eventBus.PublishAsync(createBookingEvent, cancellationToken);

            return Result.Success();
        }

        public async Task<Result<PaginatedResult<BookingDto>>> GetBookingsByUserIdAsync(
            Guid userId,
            GetBookingsRequest getBookingsRequest,
            CancellationToken cancellationToken)
        {
            var validationResult = await _getBookingRequestValidator.ValidateAsync(
                getBookingsRequest,
                cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return Result.Failure<PaginatedResult<BookingDto>>(string.Join("; ", errors));
            }

            var specification = new GetBookingsByUserIdSpecification(
                userId,
                getBookingsRequest.IsOutDate);

            return await GetPaginatedBookingsAsync(
                specification, 
                getBookingsRequest, 
                cancellationToken);
        }

        public async Task<Result<PaginatedResult<BookingDto>>> GetBookingsByHotelIdAsync(
            Guid hotelId,
            GetBookingsRequest getBookingsRequest,
            CancellationToken cancellationToken)
        {
            var validationResult = await _getBookingRequestValidator.ValidateAsync(
                getBookingsRequest,
                cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return Result.Failure<PaginatedResult<BookingDto>>(string.Join("; ", errors));
            }

            var specification = new GetBookingsByHotelIdSpecification(
                hotelId,
                getBookingsRequest.IsOutDate);

            return await GetPaginatedBookingsAsync(
                specification, 
                getBookingsRequest, 
                cancellationToken);
        }

        public async Task<Result> CancelBookingAsync(
            Guid userId,
            Guid bookingId,
            CancellationToken cancellationToken)
        {
            var specification = new GetBookingByIdSpecification(bookingId);

            var booking = await _bookingRepository.GetSingleAsync(
                specification,
                cancellationToken);

            if (booking is null)
                return Result.Failure("Booking not found");

            if (booking.UserId != userId)
                return Result.Failure("Invalid operation");

            booking.IsOutdated = true;

            await _bookingRepository.UpdateAsync(
                booking,
                cancellationToken);

            var cancelBookingEvent = _mapper.Map<CancelBookingEvent>(booking);

            await _eventBus.PublishAsync(cancelBookingEvent, cancellationToken);

            return Result.Success();
        }

        public async Task<Result> UpdateBookingAsync(
            Guid userId,
            Guid bookingId,
            CreateBookingRequest bookingRequest,
            CancellationToken cancellationToken)
        {
            var specification = new GetBookingByIdSpecification(bookingId);

            var booking = await _bookingRepository.GetSingleAsync(
                specification,
                cancellationToken);

            if (booking is null)
                return Result.Failure("Booking not found");

            if (booking.UserId != userId)
                return Result.Failure("Invalid operation");

            var validationResult = await _createBookingRequestValidator.ValidateAsync(
                bookingRequest,
                cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return Result.Failure(string.Join("; ", errors));
            }

            if (await HasBookingConflictAsync(
                booking.HotelId,
                booking.RoomId,
                bookingRequest.StartDate,
                bookingRequest.EndDate,
                booking.Id))
            {
                return Result.Failure("Date conflict");
            }

            booking.StartDate = bookingRequest.StartDate;
            booking.EndDate = bookingRequest.EndDate;

            await _bookingRepository.UpdateAsync(
                booking,
                cancellationToken);

            var updateBookingEvent = _mapper.Map<UpdateBookingEvent>(booking);

            await _eventBus.PublishAsync(updateBookingEvent, cancellationToken);

            return Result.Success();
        }

        private async Task<PaginatedResult<BookingDto>> GetPaginatedBookingsAsync(
            Specification<BookingEntity> spec,
            GetBookingsRequest request,
            CancellationToken cancellationToken)
        {
            var (items, totalPages) = await _bookingRepository.GetAsync(
                spec,
                request.PageIndex,
                request.PageSize,
                cancellationToken);

            return new PaginatedResult<BookingDto>
            {
                Items = _mapper.Map<List<BookingDto>>(items),
                CurrentPage = request.PageIndex,
                PageSize = request.PageSize,
                TotalPages = totalPages
            };
        }

        private async Task<bool> HasBookingConflictAsync(
            Guid hotelId,
            Guid roomId,
            DateTime startDate,
            DateTime endDate,
            Guid? excludeId = null)
        {
            var specification = new GetBookingByDateRangeSpecification(
                hotelId,
                roomId,
                startDate,
                endDate,
                excludeId);

            var existingBooking = await _bookingRepository.GetSingleAsync(
                specification,
                CancellationToken.None);

            return existingBooking is null ? false : true;
        }
    }
}
