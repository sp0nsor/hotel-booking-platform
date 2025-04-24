using HotelService.API.ExceptionHandling;
using HotelService.API.Mappings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace HotelService.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApi(this IServiceCollection services)
        {
            var assembles = new[]
            {
                Assembly.Load("HotelService.Application")
            };

            services.AddGrpc();

            services.AddSignalR();

            services.AddMediatR(x =>
                x.RegisterServicesFromAssemblies(assembles));

            services.AddExceptionHandler<GlobalExceptionHandler>();

            services.AddAutoMapper(typeof(RequestProfile));
            services.AddAutoMapper(typeof(GetHotelByIdProfile));
            services.AddAutoMapper(typeof(GetRoomByIdProfile));

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = false,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["JwtOptions:SecretKey"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            return services;
        }
    }
}
