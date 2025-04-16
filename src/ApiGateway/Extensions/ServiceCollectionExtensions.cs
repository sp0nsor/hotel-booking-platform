using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ApiGateway.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRedis(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
                options.InstanceName = "local";
            });

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["JwtOptions:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["JwtOptions:SecretKey"])),
                        ClockSkew = TimeSpan.Zero
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async context =>
                        {
                            using var scope = context.HttpContext.RequestServices.CreateScope();
                            var cache = scope.ServiceProvider.GetRequiredService<IDistributedCache>();

                            var accessToken = context.Request.Headers["Authorization"]
                                .FirstOrDefault()?.Split(" ").Last();

                            if (!string.IsNullOrEmpty(accessToken))
                            {
                                var jwtTokenHandler = new JwtSecurityTokenHandler();
                                var jwtToken = jwtTokenHandler.ReadJwtToken(accessToken);

                                var jwtTokenId = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

                                if (string.IsNullOrEmpty(jwtTokenId) ||
                                    string.IsNullOrEmpty(await cache.GetStringAsync($"access_{jwtTokenId}")))
                                    throw new SecurityTokenException("Invalid token");
                            }
                        }
                    };
                });

            return services;
        }
    }
}
