using BookingService.Application.Interfaces;
using BookingService.Application.Mappings;
using BookingService.Application.MassageBroker;
using BookingService.Application.Requests;
using BookingService.Application.Validators;
using FluentValidation;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookingService.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(BookingProfile));

            services.AddScoped<IBookingService, Services.BookingService>();

            services.AddScoped<IValidator<CreateBookingRequest>, CreateBookingRequestValidator>();
            services.AddScoped<IValidator<GetBookingsRequest>, GetBookingsRequestValidator>();

            var messageBrokerOptions = configuration.GetSection(nameof(MessageBrokerOptions))
                .Get<MessageBrokerOptions>();

            services.AddMassTransit(busConfiguration =>
            {
                busConfiguration.SetKebabCaseEndpointNameFormatter();

                busConfiguration.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(new Uri(messageBrokerOptions.Host), h =>
                    {
                        h.Username(messageBrokerOptions.UserName);
                        h.Password(messageBrokerOptions.Password);
                    });
                });
            });
            return services;
        }
    }
}
