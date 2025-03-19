using AutoMapper;
using HotelService.API.Grpc.Room;
using HotelService.Application.DTOs;

namespace HotelService.API.Mappings
{
    public class GetRoomByIdProfile : Profile
    {
        public GetRoomByIdProfile()
        {
            CreateMap<RoomDto, GetRoomByIdResponse>()
                .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.MoneyAmount, opt => opt.MapFrom(src => (double)src.MoneyAmount));
        }
    }
}
