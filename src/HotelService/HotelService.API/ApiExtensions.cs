using HotelService.API.ExceptionHandling;
using HotelService.DataAccess;
using Microsoft.EntityFrameworkCore;
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

        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using HotelDbContext dbContext = 
                scope.ServiceProvider.GetRequiredService<HotelDbContext>();

            dbContext.Database.Migrate();
        }
    }
}
