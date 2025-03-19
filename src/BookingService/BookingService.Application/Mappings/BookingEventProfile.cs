using AutoMapper;
using BookingService.Infrastructure.Data.Entities;
using Shared.Contracts.Bookings;

namespace BookingService.Application.Mappings
{
    public class BookingEventProfile : Profile
    {
        public BookingEventProfile()
        {
            CreateMap<BookingEntity, CancelBookingEvent>();
            CreateMap<BookingEntity, UpdateBookingEvent>();
            CreateMap<BookingEntity, CreateBookingEvent>();
        }
    }
}
