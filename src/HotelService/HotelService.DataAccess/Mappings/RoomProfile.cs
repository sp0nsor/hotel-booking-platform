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
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image.Value))
                .ForMember(dest => dest.BookedDates, opt => opt.MapFrom(src => src.BookedDates));

            CreateMap<RoomEntity, Room>()
                .ConstructUsing(src => Room.Create(
                    src.Id,
                    src.HotelId,
                    src.Capacity,
                    src.Area,
                    src.Number,
                    src.MoneyAmount,
                    src.Currency,
                    src.ImageUrl,
                    src.BookedDates.Select(d => 
                        DateRange.Create(d.StartDate, d.EndDate).Value)
                        .ToList())
                .Value);
        }
    }
}
