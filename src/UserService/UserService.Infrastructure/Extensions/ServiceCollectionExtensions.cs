using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
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

            var mongoUrl = MongoUrl.Create(configuration.GetConnectionString("HangfireDb"));
            var mongoClient = new MongoClient(mongoUrl);

            var storageOptions = new MongoStorageOptions
            {
                MigrationOptions = new MongoMigrationOptions
                {
                    MigrationStrategy = new MigrateMongoMigrationStrategy(),
                    BackupStrategy = new NoneMongoBackupStrategy()
                },
                CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection
            };

            services.AddHangfire(options =>
            {
                options.UseMongoStorage(mongoClient, mongoUrl.DatabaseName, storageOptions);
            });

            services.AddHangfireServer();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IRepository<UserEntity>, Repository<UserEntity>>();
            services.AddScoped<IRepository<RefreshTokenEntity>, Repository<RefreshTokenEntity>>();

            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IPasswordService, PasswordService>();

            return services;
        }
    }
}
