using AutoMapper;
using BookingService.Infrastructure.Data.Entities;

namespace BookingService.Application.Mappings
{
    public class BookingContextDataProfile : Profile
    {
        public BookingContextDataProfile()
        {
            CreateMap<BookingContextData, BookingEntity>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))

                .ForMember(dest => dest.HotelId, opt => opt.MapFrom(src => src.Hotel.HotelId))
                .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel.Name))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Hotel.Country))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Hotel.City))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Hotel.Street))

                .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.Room.RoomId))
                .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Room.Number))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Room.Currency))

                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.Booking.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.Booking.EndDate))

                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.GuestEmail, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.GuestFirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.GuestLastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(dest => dest.GuestPhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber));
        }
    }
}
