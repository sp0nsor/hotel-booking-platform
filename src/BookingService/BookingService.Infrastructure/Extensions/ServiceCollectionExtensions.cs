using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using BookingService.Infrastructure.Interfaces.Data;
using BookingService.Infrastructure.Data.Entities;
using BookingService.Infrastructure.Data.Repositories;

namespace BookingService.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<BookingDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString(nameof(BookingDbContext)));
            });

            services.AddScoped<IRepository<BookingEntity>, Repository<BookingEntity>>();

            return services;
        }
    }
}
