using HotelService.API.ExceptionHandling;
using System.Reflection;

namespace HotelService.API
{
    public static class ApiExtensions
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

            return services;
        }
    }
}
