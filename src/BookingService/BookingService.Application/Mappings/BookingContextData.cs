using BookingService.Application.Requests;
using BookingService.Infrastructure.Services.Grpc.Hotel;
using BookingService.Infrastructure.Services.Grpc.Room;

namespace BookingService.Application.Mappings
{
    public class BookingContextData
    {
        public GetHotelByIdResponse Hotel { get; set; }
        public GetRoomByIdResponse Room { get; set; }
        public CreateBookingRequest Booking { get; set; }
    }
}
