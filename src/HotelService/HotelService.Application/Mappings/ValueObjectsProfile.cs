using AutoMapper;
using HotelService.Application.DTOs;
using HotelService.Core.Models;
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
            CreateMap<Category, Category>();
            CreateMap<Money, Money>();
        }
    }
}
