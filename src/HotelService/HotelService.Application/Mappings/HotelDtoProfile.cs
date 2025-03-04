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
                .ConstructUsing((src, ctx) => new HotelDto(
                    src.Id,
                    src.Name,
                    src.Description,
                    src.Image.Value,
                    src.PhoneNumber.Value,
                    src.Address.Country,
                    src.Address.City,
                    src.Address.Street,
                    src.Category.Value,
                    ctx.Mapper.Map<List<RoomDto>>(src.Rooms)
                ));
        }
    }
}
