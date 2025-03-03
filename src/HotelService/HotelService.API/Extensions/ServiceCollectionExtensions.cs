using HotelService.API.ExceptionHandling;
using HotelService.API.Mappings;
using System.Reflection;

namespace HotelService.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApi(this IServiceCollection services)
        {
            var assembles = new[]
            {
                Assembly.Load("HotelService.Application")
            };

            services.AddMediatR(x =>
                x.RegisterServicesFromAssemblies(assembles));

            services.AddExceptionHandler<GlobalExceptionHandler>();

            services.AddAutoMapper(typeof(RequestProfile));

            return services;
        }
    }
}
