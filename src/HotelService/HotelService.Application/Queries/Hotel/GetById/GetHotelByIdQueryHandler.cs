using AutoMapper;
using CSharpFunctionalExtensions;
using HotelService.Application.DTOs;
using HotelService.Core.Abstractions;
using MediatR;

namespace HotelService.Application.Queries.Hotel.GetById
{
    public class GetHotelByIdQueryHandler : IRequestHandler<GetHotelByIdQuery, Result<HotelDto>>
    {
        private readonly IMapper mapper;
        private readonly IHotelRepository hotelRepository;

        public GetHotelByIdQueryHandler(
            IMapper mapper,
            IHotelRepository hotelRepository)
        {
            this.mapper = mapper;
            this.hotelRepository = hotelRepository;
        }

        public async Task<Result<HotelDto>> Handle(GetHotelByIdQuery request, CancellationToken cancellationToken)
        {
            var hotel = await hotelRepository.GetById(request.Id, cancellationToken);

            if (hotel is null)
                return Result.Failure<HotelDto>("Hotel not found");

            var hotelDto = mapper.Map<HotelDto>(hotel);

            return Result.Success(hotelDto);
        }
    }
}
