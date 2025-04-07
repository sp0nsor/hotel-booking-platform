using BookingService.Application.Requests;
using BookingService.Application.Services.Internal.Grpc.Hotel;
using BookingService.Application.Services.Internal.Grpc.Room;
using BookingService.Application.Services.Internal.Grpc.User;

namespace BookingService.Application.Mappings
{
    public class BookingContextData
    {
        public GetUserByIdResponse User { get; set; }
        public GetHotelByIdResponse Hotel { get; set; }
        public GetRoomByIdResponse Room { get; set; }
        public CreateBookingRequest Booking { get; set; }
    }
}
