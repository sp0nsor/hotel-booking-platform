using AutoMapper;
using BookingService.Application.DTOs;
using BookingService.Application.Requests;
using BookingService.Infrastructure.Data.Entities;
using BookingService.Infrastructure.Services.Grpc.Hotel;
using BookingService.Infrastructure.Services.Grpc.Room;

namespace BookingService.Application.Mappings
{
    public class BookingEntityProfile : Profile
    {
        public BookingEntityProfile()
        {
            CreateMap<BookingEntity, BookingDto>();
            CreateMap<CreateBookingRequest, BookingEntity>();

            CreateMap<(GetHotelByIdResponse getHotelByIdResponse, GetRoomByIdResponse getRoomByIdResponse, CreateBookingRequest createBookingRequest), BookingEntity>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))

                .ForMember(dest => dest.HotelId, opt => opt.MapFrom(src => src.getHotelByIdResponse.HotelId))
                .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.getHotelByIdResponse.Name))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.getHotelByIdResponse.Country))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.getHotelByIdResponse.City))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.getHotelByIdResponse.Street))

                .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.getRoomByIdResponse.RoomId))
                .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.getRoomByIdResponse.Number))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.getRoomByIdResponse.Currency))

                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.createBookingRequest.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.createBookingRequest.EndDate))

                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6")))
                .ForMember(dest => dest.GuestEmail, opt => opt.MapFrom(src => "mazie.zemlak@ethereal.email"))
                .ForMember(dest => dest.GuestFirstName, opt => opt.MapFrom(src => "GuestFirstName"))
                .ForMember(dest => dest.GuestLastName, opt => opt.MapFrom(src => "GuestLastName"))
                .ForMember(dest => dest.GuestPhoneNumber, opt => opt.MapFrom(src => "+375111111111"));
        }
    }
}
