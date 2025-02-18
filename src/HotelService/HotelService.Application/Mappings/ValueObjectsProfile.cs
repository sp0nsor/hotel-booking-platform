using AutoMapper;
using HotelService.Core.ValueObjects;

namespace HotelService.Application.Mappings
{
    public class ValueObjectsProfile : Profile
    {
        public ValueObjectsProfile()
        {
            CreateMap<Image, Image>();
            CreateMap<PhoneNumber, PhoneNumber>();
            CreateMap<Address, Address>();
            CreateMap<PriceCategory, PriceCategory>();
            CreateMap<DateRange, DateRange>();
            CreateMap<Money, Money>();
        }
    }
}
