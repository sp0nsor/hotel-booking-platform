using HotelService.Application.Interfaces;
using HotelService.Infrastructure.Caching;
using HotelService.Infrastructure.Files;
using HotelService.Infrastructure.MessageBroker.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelService.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IImageService, ImageService>();

            services.AddMassTransit(busConfiguration =>
            {
                busConfiguration.AddConsumer<CreateBookingConsumer>();

                busConfiguration.SetKebabCaseEndpointNameFormatter();

                busConfiguration.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(new Uri("amqp://guest:guest@bookings-queue:5672"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ConfigureEndpoints(ctx);
                });
            });

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
                options.InstanceName = "local";
            });

            return services;
        }
    }
}
