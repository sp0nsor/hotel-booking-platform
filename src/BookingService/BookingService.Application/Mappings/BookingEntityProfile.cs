using AutoMapper;
using BookingService.Application.DTOs;
using BookingService.Application.Requests;
using BookingService.Infrastructure.Data.Entities;

namespace BookingService.Application.Mappings
{
    public class BookingEntityProfile : Profile
    {
        public BookingEntityProfile()
        {
            CreateMap<BookingEntity, BookingDto>();

            CreateMap<CreateBookingRequest, BookingEntity>();
        }
    }
}
