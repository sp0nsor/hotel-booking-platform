using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Infrastructure.Data.Entities;
using UserService.Infrastructure.Data.Repositories;
using UserService.Infrastructure.Interfaces.Data;
using UserService.Infrastructure.Interfaces.Services;
using UserService.Infrastructure.Services;

namespace UserService.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<UsersDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString(nameof(UsersDbContext)));
            });

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
                options.InstanceName = "local";
            });

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IRepository<UserEntity>, Repository<UserEntity>>();
            services.AddScoped<IRepository<RefreshTokenEntity>, Repository<RefreshTokenEntity>>();

            services.AddScoped<IConfirmCodeService, ConfirmCodeService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            return services;
        }
    }
}
