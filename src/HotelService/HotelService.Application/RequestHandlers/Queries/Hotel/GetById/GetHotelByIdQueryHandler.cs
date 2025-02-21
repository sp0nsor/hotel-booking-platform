using AutoMapper;
using CSharpFunctionalExtensions;
using HotelService.Application.DTOs;
using HotelService.Core.Abstractions;
using MediatR;

namespace HotelService.Application.RequestHandlers.Queries.Hotel.GetById
{
    public class GetHotelByIdQueryHandler : IRequestHandler<GetHotelByIdQuery, Result<HotelDto>>
    {
        private readonly IMapper mapper;
        private readonly IRepository<Core.Models.Hotel> hotelRepository;

        public GetHotelByIdQueryHandler(
            IMapper mapper,
            IRepository<Core.Models.Hotel> hotelRepository)
        {
            this.mapper = mapper;
            this.hotelRepository = hotelRepository;
        }

        public async Task<Result<HotelDto>> Handle(
            GetHotelByIdQuery request,
            CancellationToken cancellationToken)
        {
            var hotel = await hotelRepository.GetByIdWithIncludeAsync(
                request.Id, 
                cancellationToken,
                includeProperties: "Rooms");

            if (hotel is null)
                return Result.Failure<HotelDto>("Hotel not found");

            var hotelDto = mapper.Map<HotelDto>(hotel);

            return Result.Success(hotelDto);
        }
    }
}
