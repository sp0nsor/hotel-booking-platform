using AutoMapper;
using BookingService.Application.DTOs;
using BookingService.Application.Requests;
using BookingService.Infrastructure.Data.Entities;

namespace BookingService.Application.Mappings
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<BookingDataRequest, BookingEntity>();
            CreateMap<BookingDataRequest, BookingEntity>();

            CreateMap<BookingEntity, BookingDto>();
        }
    }
}
