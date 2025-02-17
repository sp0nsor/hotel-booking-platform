using CSharpFunctionalExtensions;
using HotelService.Application.DTOs;
using HotelService.Core.Abstractions;
using MediatR;

namespace HotelService.Application.Queries.Hotel.GetById
{
    public class GetHotelByIdQueryHandler : IRequestHandler<GetHotelByIdQuery, Result<HotelDto>>
    {
        private readonly IHotelRepository hotelRepository;

        public GetHotelByIdQueryHandler(IHotelRepository hotelRepository)
        {
            this.hotelRepository = hotelRepository;
        }

        public async Task<Result<HotelDto>> Handle(GetHotelByIdQuery request, CancellationToken cancellationToken)
        {
            var hotel = await hotelRepository.GetById(request.Id, cancellationToken);

            if (hotel is null)
                return Result.Failure<HotelDto>("Hotel not found");

            // map hotel to hotel dto

            return Result.Success<HotelDto>(new());
        }
    }
}
