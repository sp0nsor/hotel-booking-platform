using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Interfaces;
using UserService.Application.Mappings;
using UserService.Application.Requests;
using UserService.Application.Services;
using UserService.Application.Validators;

namespace UserService.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(UserEntityProfile));
            
            services.AddScoped<IAccessTokenService, AccessTokenService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IUserService, Services.UserService>();

            services.AddScoped<IValidator<RegisterUserRequest>, RegisterUserRequestValidator>();
            services.AddScoped<IValidator<LoginUserRequest>, LoginUserRequestValidator>();
            services.AddScoped<IValidator<UpdateUserInfoRequest>, UpdateUserInfoRequestValidator>();
            services.AddScoped<IValidator<ConfirmUserRequest>, ConfirmUserRequestValidator>();

            return services;
        }
    }
}
