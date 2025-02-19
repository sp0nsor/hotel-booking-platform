using HotelService.Application.DTOs;
using HotelService.Application.Interfaces;
using HotelService.Application.Mappings;
using HotelService.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HotelService.Application
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddScoped<IImageService, ImageService>();

            services.AddAutoMapper(typeof(HotelDtoProfile).Assembly);
            services.AddAutoMapper(typeof(RoomDto).Assembly);
            services.AddAutoMapper(typeof(ValueObjectsProfile).Assembly);

            return services;
        }
    }
}
