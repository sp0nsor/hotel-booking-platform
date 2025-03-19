using AutoMapper;
using HotelService.API.Grpc.Hotel;
using HotelService.Application.DTOs;

namespace HotelService.API.Mappings
{
    public class GetHotelByIdProfile : Profile
    {
        public GetHotelByIdProfile()
        {
            CreateMap<HotelDto, GetHotelByIdResponse>()
                .ForMember(dest => dest.HotelId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
