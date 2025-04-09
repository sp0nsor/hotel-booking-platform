using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Interfaces.Internal;
using UserService.Application.Interfaces.Public;
using UserService.Application.Mappings;
using UserService.Application.Requests;
using UserService.Application.Services.Internal;
using UserService.Application.Validators;

namespace UserService.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(UserEntityProfile));

            services.AddScoped<IConfirmCodeService, ConfirmCodeService>();
            services.AddScoped<IAccessTokenService, AccessTokenService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IUserService, Services.Public.UserService>();

            services.AddScoped<IValidator<RegisterUserRequest>, RegisterUserRequestValidator>();
            services.AddScoped<IValidator<LoginUserRequest>, LoginUserRequestValidator>();
            services.AddScoped<IValidator<UpdateUserInfoRequest>, UpdateUserInfoRequestValidator>();
            services.AddScoped<IValidator<ConfirmUserRequest>, ConfirmUserRequestValidator>();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IPasswordService, PasswordService>();

            return services;
        }
    }
}
