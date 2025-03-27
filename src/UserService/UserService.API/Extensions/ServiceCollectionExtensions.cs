using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using UserService.Application.Options;

namespace UserService.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var tokenOptions = configuration
                .GetSection(nameof(AccessTokenOptions))
                    .Get<AccessTokenOptions>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = tokenOptions.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes(tokenOptions.SecretKey)),
                    ClockSkew = TimeSpan.Zero

                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Headers["Authorization"]
                            .FirstOrDefault()?.Split(" ").Last();

                        if (!string.IsNullOrEmpty(accessToken))
                            context.Token = accessToken;

                        Console.WriteLine(accessToken);

                        return Task.CompletedTask;
                    },

                    OnTokenValidated = async context =>
                    {
                        var accessToken = context.Request.Headers["Authorization"]
                            .FirstOrDefault()?.Split(" ").Last();

                        if (new JwtSecurityTokenHandler().ReadJwtToken(accessToken) is JwtSecurityToken jwtToken)
                        {
                            var cache = context.HttpContext.RequestServices.GetRequiredService<IDistributedCache>();

                            var jwtTokenId = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

                            var existToken = await cache.GetStringAsync($"access_{jwtTokenId}");

                            if (string.IsNullOrEmpty(existToken))
                                throw new SecurityTokenException("Invalid token");

                        }
                    }
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy =>
                    policy.RequireRole("Admin")
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));

                options.AddPolicy("UserPolicy", policy =>
                    policy.RequireRole("User")
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));
            });

            return services;
        }
    }
}
