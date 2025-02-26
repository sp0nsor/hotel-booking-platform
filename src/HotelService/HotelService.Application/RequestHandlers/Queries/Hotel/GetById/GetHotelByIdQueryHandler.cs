using AutoMapper;
using CSharpFunctionalExtensions;
using HotelService.Application.DTOs;
using HotelService.Application.Interfaces;
using HotelService.Core.Abstractions;
using HotelService.Core.ValueObjects;
using MediatR;

namespace HotelService.Application.RequestHandlers.Queries.Hotel.GetById
{
    public class GetHotelByIdQueryHandler : IRequestHandler<GetHotelByIdQuery, Result<HotelDto>>
    {
        private readonly IMapper mapper;
        private readonly IRedisCacheService cacheService;
        private readonly IRepository<Core.Models.Hotel> hotelRepository;

        public GetHotelByIdQueryHandler(
            IMapper mapper,
            IRedisCacheService cacheService,
            IRepository<Core.Models.Hotel> hotelRepository)
        {
            this.mapper = mapper;
            this.cacheService = cacheService;
            this.hotelRepository = hotelRepository;
        }

        public async Task<Result<HotelDto>> Handle(
            GetHotelByIdQuery request,
            CancellationToken cancellationToken)
        {
            var cachedKey = $"hotel_{request.Id}";
            var cachedHotel = await cacheService.GetAsync<HotelDto>(cachedKey);

            if (cachedHotel != null)
                return Result.Success(cachedHotel);

            var hotel = await hotelRepository.GetByIdAsync(
                request.Id, 
                cancellationToken,
                includeProperties: "Rooms");

            if (hotel is null)
                return Result.Failure<HotelDto>("Hotel not found");

            var hotelDto = mapper.Map<HotelDto>(hotel);

            await cacheService.SetAsync(
                cachedKey,
                hotelDto,
                TimeSpan.FromDays(1));

            return Result.Success(hotelDto);
        }
    }
}
