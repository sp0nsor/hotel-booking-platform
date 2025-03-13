using AutoMapper;
using BookingService.Application.DTOs;
using BookingService.Application.Requests;
using BookingService.Infrastructure.Data.Entities;
using Shared.Contracts.Bookings;

namespace BookingService.Application.Mappings
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<CreateBookingRequest, BookingEntity>();

            CreateMap<BookingEntity, CancelBookingEvent>();
            CreateMap<BookingEntity, UpdateBookingEvent>();
            CreateMap<BookingEntity, CreateBookingEvent>();

            CreateMap<BookingEntity, BookingDto>();
        }
    }
}
