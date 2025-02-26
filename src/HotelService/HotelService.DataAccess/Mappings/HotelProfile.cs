using AutoMapper;
using HotelService.Core.Models;
using HotelService.DataAccess.Entities;

namespace HotelService.DataAccess.Mappings
{
    public class HotelProfile : Profile
    {
        public HotelProfile()
        {
            CreateMap<Hotel, HotelEntity>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber.Value))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Address.Country))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image.Value))
                .ForMember(dest => dest.PriceCategory, opt => opt.MapFrom(src => src.Category.Value))
                .ForMember(dest => dest.Rooms, opt => opt.MapFrom(src => src.Rooms));

            CreateMap<HotelEntity, Hotel>()
            .ConstructUsing((src, ctx) => Hotel.Create(
                src.Id,
                src.Name,
                src.Description,
                src.PhoneNumber,
                src.Country,
                src.City,
                src.Street,
                src.PriceCategory,
                src.ImageUrl,
                src.Rooms.Select(roomEntity => ctx.Mapper.Map<Room>(roomEntity)).ToList()
            ).Value);
        }
    }
}
