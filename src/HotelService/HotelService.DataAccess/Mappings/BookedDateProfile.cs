using AutoMapper;
using HotelService.Core.ValueObjects;
using HotelService.DataAccess.Entities;

namespace HotelService.DataAccess.Mappings
{
    public class BookedDateProfile : Profile
    {
        public BookedDateProfile()
        {
            CreateMap<BookedDateEntity, DateRange>()
                .ConstructUsing(src => DateRange.Create(src.StartDate, src.EndDate).Value);

            CreateMap<DateRange, BookedDateEntity>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate));
        }
    }
}
