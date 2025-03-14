using AutoMapper;
using HotelService.Application.RequestHandlers.Commands.Booking.Create;
using Shared.Contracts.Bookings;

namespace HotelService.Infrastructure.Mappings
{
    internal class CreateBookingProfile : Profile
    {
        public CreateBookingProfile()
        {
            CreateMap<CreateBookingEvent, CreateBookingCommand>();
        }
    }
}
