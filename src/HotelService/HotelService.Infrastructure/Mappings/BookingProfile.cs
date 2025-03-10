using AutoMapper;
using HotelService.Application.RequestHandlers.Commands.Booking.Cancel;
using HotelService.Application.RequestHandlers.Commands.Booking.Create;
using HotelService.Application.RequestHandlers.Commands.Booking.Update;
using Shared.Contracts.Bookings;

namespace HotelService.Infrastructure.Mappings
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<CreateBookingEvent, CreateBookingCommand>();
            CreateMap<UpdateBookingEvent, UpdateBookingCommand>();
            CreateMap<CancelBookingEvent, CancelBookingCommand>();
        }
    }
}
