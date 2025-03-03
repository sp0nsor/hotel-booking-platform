using AutoMapper;
using HotelService.Core.Models;
using HotelService.DataAccess.Entities;

namespace HotelService.DataAccess.Mappings
{
    public class BookingPeriodProfile : Profile
    {
        public BookingPeriodProfile()
        {
            CreateMap<BookingPeriodEntity, BookingPeriod>();
            CreateMap<BookingPeriod, BookingPeriodEntity>();
        }
    }
}
