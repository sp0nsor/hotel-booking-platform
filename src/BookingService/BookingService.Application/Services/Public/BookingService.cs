using AutoMapper;
using BookingService.Application.DTOs;
using BookingService.Application.Interfaces.Internal;
using BookingService.Application.Interfaces.Public;
using BookingService.Application.Mappings;
using BookingService.Application.Requests;
using BookingService.Infrastructure.Data.Entities;
using BookingService.Infrastructure.Data.Specifications;
using BookingService.Infrastructure.Interfaces.Data;
using CSharpFunctionalExtensions;
using FluentValidation;
using Hangfire;
using Shared.Contracts.Bookings;

namespace BookingService.Application.Services.Public
{
    public class BookingService : IBookingService
    {
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IEventBus _eventBus;
        private readonly IUserGrpcService _userGrpcService;
        private readonly IHotelGrpcService _hotelGrpcService;
        private readonly IRoomGrpcService _roomGrpcService;
        private readonly IRepository<BookingEntity> _bookingRepository;
        private readonly IValidator<GetBookingsRequest> _getBookingRequestValidator;
        private readonly IValidator<CreateBookingRequest> _createBookingRequestValidator;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public BookingService(
            IMapper mapper,
            IEmailService emailService,
            IValidator<GetBookingsRequest> getBookingRequestValidator,
            IBackgroundJobClient backgroundJobClient,
            IHotelGrpcService hotelGrpcService,
            IEventBus eventBus,
            IRepository<BookingEntity> bookingRepository,
            IValidator<CreateBookingRequest> dataRequestValidator,
            IRoomGrpcService roomGrpcService,
            IUserGrpcService userGrpcService)
        {
            _eventBus = eventBus;
            _mapper = mapper;
            _emailService = emailService;
            _createBookingRequestValidator = dataRequestValidator;
            _bookingRepository = bookingRepository;
            _backgroundJobClient = backgroundJobClient;
            _getBookingRequestValidator = getBookingRequestValidator;
            _hotelGrpcService = hotelGrpcService;
            _roomGrpcService = roomGrpcService;
            _userGrpcService = userGrpcService;
        }

        public async Task<Result> CreateBookingAsync(
            Guid userId,
            Guid hotelId,
            Guid roomId,
            CreateBookingRequest bookingRequest,
            CancellationToken cancellationToken)
        {
            var validationResult = await _createBookingRequestValidator.ValidateAsync(
                bookingRequest,
                cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return Result.Failure(string.Join("; ", errors));
            }

            if (await HasBookingConflictAsync(
                hotelId,
                roomId,
                bookingRequest.StartDate,
                bookingRequest.EndDate))
            {
                return Result.Failure("This dates already booked");
            }

            var getHotelByIdResult = _hotelGrpcService.GetHotelById(hotelId);

            if (getHotelByIdResult.IsFailure)
                return Result.Failure(getHotelByIdResult.Error);

            var getRoomByIdResult = _roomGrpcService.GetRoomById(roomId, hotelId);

            if (getRoomByIdResult.IsFailure)
                return Result.Failure(getRoomByIdResult.Error);

            var getUserByIdResult = _userGrpcService.GetUserById(userId);

            if (getUserByIdResult.IsFailure)
                return Result.Failure(getUserByIdResult.Error);

            var bookingContext = new BookingContextData
            {
                User = getUserByIdResult.Value,
                Hotel = getHotelByIdResult.Value,
                Room = getRoomByIdResult.Value,
                Booking = bookingRequest
            };

            var booking = _mapper.Map<BookingEntity>(bookingContext);

            var numberOfDays = (booking.EndDate.Date - booking.StartDate.Date).Days;

            booking.TotalPrice = (decimal)(numberOfDays * getRoomByIdResult.Value.MoneyAmount);

            await _bookingRepository.CreateAsync(
                booking,
                cancellationToken);

            var createBookingEvent = _mapper.Map<CreateBookingEvent>(booking);

            await _eventBus.PublishAsync(createBookingEvent, cancellationToken);

            _backgroundJobClient.Enqueue(() =>
               _emailService.SendEmailAsync(
                    booking.GuestEmail,
                    "Booking status",
                    "Your booking was created successfully",
                    CancellationToken.None
               )
            );

            _backgroundJobClient.Schedule(() =>
                CancelBookingAsync(
                    booking.UserId,
                    booking.Id,
                    CancellationToken.None
                ),
                booking.EndDate
            );

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

            _backgroundJobClient.Enqueue(() =>
                _emailService.SendEmailAsync(
                booking.GuestEmail,
                "Booking status",
                "Your booking was cancelled successfully",
                CancellationToken.None
                )
            );

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

            if (booking.IsOutdated)
                return Result.Failure("Booking already cancelled");

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
