using HotelService.Application.DTOs;
using HotelService.Application.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace HotelService.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(HotelDtoProfile));
            services.AddAutoMapper(typeof(RoomDto));
            services.AddAutoMapper(typeof(BookedDateDtoProfile));

            return services;
        }
    }
}
