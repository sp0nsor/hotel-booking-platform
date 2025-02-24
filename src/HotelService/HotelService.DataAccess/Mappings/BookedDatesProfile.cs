using AutoMapper;
using HotelService.Core.Models;
using HotelService.DataAccess.Entities;

namespace HotelService.DataAccess.Mappings
{
    public class BookedDatesProfile : Profile
    {
        public BookedDatesProfile()
        {
            CreateMap<BookedDatesEntity, BookedDates>();
            CreateMap<BookedDates, BookedDatesEntity>();
        }
    }
}
