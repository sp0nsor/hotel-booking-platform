using BookingService.Application.Interfaces.Internal;
using BookingService.Application.Interfaces.Public;
using BookingService.Application.Mappings;
using BookingService.Application.Options;
using BookingService.Application.Requests;
using BookingService.Application.Services.Internal;
using BookingService.Application.Services.Internal.Grpc.Hotel;
using BookingService.Application.Services.Internal.Grpc.Room;
using BookingService.Application.Services.Internal.Grpc.User;
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
            services.AddAutoMapper(typeof(BookingEventProfile));
            services.AddAutoMapper(typeof(BookingEntityProfile));

            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUserGrpcService, UserGrpcService>();
            services.AddScoped<IHotelGrpcService, HotelGrpcService>();
            services.AddScoped<IRoomGrpcService, RoomGrpcService>();

            services.AddScoped<IBookingService, Services.Public.BookingService>();
            services.AddScoped<IEventBus, EventBus>();

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

            services.AddGrpcClientWithCustomHandler<RoomService.RoomServiceClient>("https://hotel-service:8081");
            services.AddGrpcClientWithCustomHandler<HotelService.HotelServiceClient>("https://hotel-service:8081");
            services.AddGrpcClientWithCustomHandler<UserService.UserServiceClient>("https://user-service:8081");

            return services;
        }

        private static IServiceCollection AddGrpcClientWithCustomHandler<TClient>(
            this IServiceCollection services, string serviceUrl)
            where TClient : class
        {
            services.AddGrpcClient<TClient>(options =>
            {
                options.Address = new Uri(serviceUrl);
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
                return handler;
            });

            return services;
        }
    }
}
