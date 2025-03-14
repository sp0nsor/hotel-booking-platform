using AutoMapper;
using HotelService.Application.RequestHandlers.Commands.Booking.Cancel;
using Shared.Contracts.Bookings;

namespace HotelService.Infrastructure.Mappings
{
    public class CancelBookingProfile : Profile
    {
        public CancelBookingProfile()
        {
            CreateMap<CancelBookingEvent, CancelBookingCommand>();
        }
    }
}
