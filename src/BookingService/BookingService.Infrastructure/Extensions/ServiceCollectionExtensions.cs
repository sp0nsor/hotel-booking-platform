using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using BookingService.Infrastructure.Interfaces.Data;
using BookingService.Infrastructure.Data.Entities;
using BookingService.Infrastructure.Data.Repositories;
using BookingService.Infrastructure.MassageBroker;
using MassTransit;
using BookingService.Infrastructure.Interfaces.MessageBroker;

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

            services.AddMassTransit(busConfiguration =>
            {
                busConfiguration.SetKebabCaseEndpointNameFormatter();

                busConfiguration.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(new Uri("amqp://guest:guest@bookings-queue:5672"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                });
            });

            services.AddTransient<IEventBus, EventBus>();

            return services;
        }
    }
}
