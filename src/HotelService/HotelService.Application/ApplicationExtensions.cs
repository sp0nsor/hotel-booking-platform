using HotelService.Application.DTOs;
using HotelService.Application.Interfaces;
using HotelService.Application.Mappings;
using HotelService.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HotelService.Application
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IRedisCacheService, RedisCacheService>();

            services.AddAutoMapper(typeof(HotelDtoProfile));
            services.AddAutoMapper(typeof(RoomDto));
            services.AddAutoMapper(typeof(BookedDateDtoProfile));

            return services;
        }
    }
}
