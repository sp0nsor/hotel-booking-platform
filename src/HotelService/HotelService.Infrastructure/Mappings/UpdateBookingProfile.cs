using AutoMapper;
using HotelService.Application.RequestHandlers.Commands.Booking.Update;
using Shared.Contracts.Bookings;

namespace HotelService.Infrastructure.Mappings
{
    public class UpdateBookingProfile : Profile
    {
        public UpdateBookingProfile()
        {
            CreateMap<UpdateBookingEvent, UpdateBookingCommand>();
        }
    }
}
