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
                .ConstructUsing((src, ctx) => new RoomDto(
                    src.Id,
                    src.HotelId,
                    src.Capacity,
                    src.Area,
                    src.Number,
                    src.Image.Value,
                    src.Price.Amount,
                    src.Price.Currency,
                    ctx.Mapper.Map<List<BookedDatesDto>>(src.BookedDates)
                ));
        }
    }
}
