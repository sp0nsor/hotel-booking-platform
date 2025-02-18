using AutoMapper;
using HotelService.Application.DTOs;
using HotelService.Core.Models;

namespace HotelService.Application.Mappings
{
    public class HotelDtoProfile : Profile
    {
        public HotelDtoProfile()
        {
            CreateMap<Hotel, HotelDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Rooms, opt => opt.MapFrom(src => src.Rooms));
        }
    }
}
