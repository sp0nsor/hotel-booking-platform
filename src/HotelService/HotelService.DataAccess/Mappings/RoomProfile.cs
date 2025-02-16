using AutoMapper;
using HotelService.Core.Models;
using HotelService.Core.ValueObjects;
using HotelService.DataAccess.Entities;

namespace HotelService.DataAccess.Mappings
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<Room, RoomEntity>()
                .ForMember(dest => dest.MoneyAmount, opt => opt.MapFrom(src => src.Price.Amount))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Price.Currency))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image.Url))
                .ForMember(dest => dest.BookedDateEntities, opt => opt.MapFrom(src => src.BookedDates));

            CreateMap<RoomEntity, Room>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => Money.Create(src.MoneyAmount, src.Currency).Value))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => Image.Create(src.ImageUrl).Value))
                .ForMember(dest => dest.BookedDates, opt => opt.MapFrom(src => src.BookedDateEntities));
        }
    }
}
