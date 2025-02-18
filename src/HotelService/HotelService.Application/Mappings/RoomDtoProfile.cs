using AutoMapper;
using HotelService.Application.DTOs;
using HotelService.Core.Models;

namespace HotelService.Application.Mappings
{
    public class RoomDtoProfile : Profile
    {
        public RoomDtoProfile()
        {
            CreateMap<Room, RoomDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Capacity, opt => opt.MapFrom(src => src.Capacity))
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Area))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.BookedDates, opt => opt.MapFrom(src => src.BookedDates));
        }
    }
}
