using AutoMapper;
using HotelService.Application.DTOs;
using HotelService.Core.Models;

namespace HotelService.Application.Mappings
{
    public class BookedDateDtoProfile : Profile
    {
        public BookedDateDtoProfile()
        {
            CreateMap<BookedDates, BookedDatesDto>();
        }
    }
}
