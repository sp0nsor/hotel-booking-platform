using BookingService.Application.Interfaces;
using BookingService.Application.Mappings;
using BookingService.Application.Requests;
using BookingService.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BookingService.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(BookingProfile));

            services.AddScoped<IBookingService, Services.BookingService>();

            services.AddScoped<IValidator<CreateBookingRequest>, BookingDatesRequestValidator>();
            services.AddScoped<IValidator<GetBookingsRequest>, GetBookingsRequestValidator>();

            return services;
        }
    }
}
