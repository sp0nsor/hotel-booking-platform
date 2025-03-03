using AutoMapper;
using HotelService.Core.Models;
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
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image.Value))
                .ForMember(dest => dest.BookingPeriods, opt => opt.MapFrom(src => src.BookingPeriods));

            CreateMap<RoomEntity, Room>()
                .ConstructUsing((src, ctx) => Room.Create(
                    src.Id,
                    src.HotelId,
                    src.Capacity,
                    src.Area,
                    src.Number,
                    src.MoneyAmount,
                    src.Currency,
                    src.ImageUrl,
                    src.BookingPeriods.Select(bookingPeriod => ctx.Mapper.Map<BookingPeriod>(bookingPeriod)).ToList()
                ).Value);
        }
    }
}
