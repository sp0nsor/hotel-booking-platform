using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using UserService.Infrastructure.Options;

namespace UserService.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var accessTokenOptions = configuration
                .GetSection(nameof(AccessTokenOptions))
                .Get<AccessTokenOptions>();

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
                        ValidIssuer = accessTokenOptions.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(accessTokenOptions.SecretKey)),
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

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy =>
                    policy.RequireRole("Admin"));

                options.AddPolicy("UserPolicy", policy =>
                    policy.RequireRole("User"));
            });

            return services;
        }
    }
}
