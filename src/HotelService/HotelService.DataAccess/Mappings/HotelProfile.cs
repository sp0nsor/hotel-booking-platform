using AutoMapper;
using HotelService.Core.Models;
using HotelService.Core.ValueObjects;
using HotelService.DataAccess.Entities;
namespace HotelService.DataAccess.Mappings
{
    public class HotelProfile : Profile
    {
        public HotelProfile()
        {
            CreateMap<Hotel, HotelEntity>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber.Number))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Address.Country))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image.Url))
                .ForMember(dest => dest.PriceCategory, opt => opt.MapFrom(src => src.Category.Value))
                .ForMember(dest => dest.Rooms, opt => opt.MapFrom(src => src.Rooms));

            CreateMap<HotelEntity, Hotel>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => PhoneNumber.Create(src.PhoneNumber).Value))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => Address.Create(src.Country, src.City, src.Street).Value))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => PriceCategory.Create(src.PriceCategory).Value))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => Image.Create(src.ImageUrl).Value))
                .ForMember(dest => dest.Rooms, opt => opt.MapFrom(src => src.Rooms));
        }
    }
}
