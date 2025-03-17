using HotelService.Application.Interfaces;
using HotelService.Infrastructure.Caching;
using HotelService.Infrastructure.Files;
using HotelService.Infrastructure.Mappings;
using HotelService.Infrastructure.MessageBroker;
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
            services.AddAutoMapper(typeof(CreateBookingProfile));
            services.AddAutoMapper(typeof(UpdateBookingProfile));
            services.AddAutoMapper(typeof(CancelBookingProfile));

            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IImageService, ImageService>();

            var messageBrokerOptions = configuration.GetSection(nameof(MessageBrokerOptions))
                .Get<MessageBrokerOptions>();

            services.AddMassTransit(busConfiguration =>
            {
                busConfiguration.AddConsumer<CreateBookingConsumer>();
                busConfiguration.AddConsumer<UpdateBookingConsumer>();
                busConfiguration.AddConsumer<CancelBookingConsumer>();

                busConfiguration.SetKebabCaseEndpointNameFormatter();

                busConfiguration.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(new Uri(messageBrokerOptions.Host), h =>
                    {
                        h.Username(messageBrokerOptions.UserName);
                        h.Password(messageBrokerOptions.Password);
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
