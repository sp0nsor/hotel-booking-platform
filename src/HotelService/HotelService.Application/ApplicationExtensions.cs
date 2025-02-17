using HotelService.Application.Interfaces;
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

            return services;
        }
    }
}
