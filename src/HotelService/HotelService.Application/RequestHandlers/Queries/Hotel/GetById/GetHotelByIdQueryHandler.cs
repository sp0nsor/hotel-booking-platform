using AutoMapper;
using CSharpFunctionalExtensions;
using HotelService.Application.DTOs;
using HotelService.Application.Interfaces;
using HotelService.Core.Abstractions;
using MediatR;

namespace HotelService.Application.RequestHandlers.Queries.Hotel.GetById
{
    public class GetHotelByIdQueryHandler
        : IRequestHandler<GetHotelByIdQuery, Result<HotelDto>>
    {
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly IRepository<Core.Models.Hotel> _hotelRepository;

        public GetHotelByIdQueryHandler(
            IMapper mapper,
            ICacheService cacheService,
            IRepository<Core.Models.Hotel> hotelRepository)
        {
            _mapper = mapper;
            _cacheService = cacheService;
            _hotelRepository = hotelRepository;
        }

        public async Task<Result<HotelDto>> Handle(
            GetHotelByIdQuery request,
            CancellationToken cancellationToken)
        {
            var cachedKey = $"hotel_{request.Id}";

            var cachedHotel = await _cacheService.GetAsync<HotelDto>(
                cachedKey,
                cancellationToken);

            if (cachedHotel != null)
                return Result.Success(cachedHotel);

            var hotel = await _hotelRepository.GetByIdAsync(
                request.Id,
                cancellationToken,
                includeProperties: "Rooms");

            if (hotel is null)
                return Result.Failure<HotelDto>("Hotel not found");

            var hotelDto = _mapper.Map<HotelDto>(hotel);

            await _cacheService.SetAsync(
                cachedKey,
                hotelDto,
                cancellationToken,
                TimeSpan.FromDays(1));

            return Result.Success(hotelDto);
        }
    }
}
