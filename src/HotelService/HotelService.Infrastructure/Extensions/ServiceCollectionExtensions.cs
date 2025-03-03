using HotelService.Application.Interfaces;
using HotelService.Infrastructure.Caching;
using HotelService.Infrastructure.Files;
using Microsoft.Extensions.DependencyInjection;

namespace HotelService.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services)
        {
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IImageService, ImageService>();

            return services;
        }
    }
}
